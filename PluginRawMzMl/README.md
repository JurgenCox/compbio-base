# PluginMzMl

Downloaded `mzML1.1.1_idx.xsd` schema from http://psidev.info/mzML.
Generated C# classes from schema using

	xsd /classes mzML1.1.1_idx.xsd /n:PluginRawMzMl

Manually changed `run.startTimeStamp` to `string` rather than `DateTime` in
order to be more fault tolerant.

## Compression

PluginMzMl supports non-standard compression with MsNumpress.