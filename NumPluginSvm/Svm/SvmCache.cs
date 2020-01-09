using System;

namespace NumPluginSvm.Svm{
	/// <summary>
	/// Kernel Cache
	///
	/// l is the number of total data items
	/// size is the cache size limit in bytes
	/// </summary>
	internal class SvmCache{
		private long size;

		private class HeadT{
			internal HeadT prev; // a cicular list
			internal HeadT next; // a cicular list
			internal float[] data;
			internal int len; // data[0,len) is cached in this entry
		}

		private readonly HeadT[] head;
		private readonly HeadT lruHead;

		internal SvmCache(int l1, long size1){
			int l = l1;
			size = size1;
			head = new HeadT[l];
			for (int i = 0; i < l; i++){
				head[i] = new HeadT();
			}
			size /= 4;
			size -= l*(16/4); // sizeof(head_t) == 16
			size = Math.Max(size, 2*(long) l); // cache must be large enough for two columns
			lruHead = new HeadT();
			lruHead.next = lruHead.prev = lruHead;
		}

		private static void LruDelete(HeadT h){
			// delete from current location
			h.prev.next = h.next;
			h.next.prev = h.prev;
		}

		private void LruInsert(HeadT h){
			// insert to last position
			h.next = lruHead;
			h.prev = lruHead.prev;
			h.prev.next = h;
			h.next.prev = h;
		}

		// request data [0,len)
		// return some position p where [p,len) need to be filled
		// (p >= len if nothing needs to be filled)
		// java: simulate pointer using single-element array
		internal int GetData(int index, float[][] data, int len){
			HeadT h = head[index];
			if (h.len > 0){
				LruDelete(h);
			}
			int more = len - h.len;
			if (more > 0){
				// free old space
				while (size < more){
					HeadT old = lruHead.next;
					LruDelete(old);
					size += old.len;
					old.data = null;
					old.len = 0;
				}
				// allocate new space
				float[] newData = new float[len];
				if (h.data != null){
					Array.Copy(h.data, 0, newData, 0, h.len);
				}
				h.data = newData;
				size -= more;
				do{
					int tmp = h.len;
					h.len = len;
					len = tmp;
				} while (false);
			}
			LruInsert(h);
			data[0] = h.data;
			return len;
		}

		internal void SwapIndex(int i, int j){
			if (i == j){
				return;
			}
			if (head[i].len > 0){
				LruDelete(head[i]);
			}
			if (head[j].len > 0){
				LruDelete(head[j]);
			}
			do{
				float[] tmp = head[i].data;
				head[i].data = head[j].data;
				head[j].data = tmp;
			} while (false);
			do{
				int tmp = head[i].len;
				head[i].len = head[j].len;
				head[j].len = tmp;
			} while (false);
			if (head[i].len > 0){
				LruInsert(head[i]);
			}
			if (head[j].len > 0){
				LruInsert(head[j]);
			}
			if (i > j){
				do{
					int tmp = i;
					i = j;
					j = tmp;
				} while (false);
			}
			for (HeadT h = lruHead.next; h != lruHead; h = h.next){
				if (h.len > i){
					if (h.len > j){
						do{
							float tmp = h.data[i];
							h.data[i] = h.data[j];
							h.data[j] = tmp;
						} while (false);
					} else{
						// give up
						LruDelete(h);
						size += h.len;
						h.data = null;
						h.len = 0;
					}
				}
			}
		}
	}
}