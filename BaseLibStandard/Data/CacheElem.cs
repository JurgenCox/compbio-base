namespace BaseLibS.Data{
	internal class CacheElem<K, V>{
		internal CacheElem<K, V> previous;
		internal CacheElem<K, V> next;
		internal K index;
		internal V data;

		internal CacheElem(K index, V data){
			this.index = index;
			this.data = data;
		}
	}
}