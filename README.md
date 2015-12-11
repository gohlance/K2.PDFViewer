# K2.PDFViewer

Overview
This is a custom control for K2 Smartform. This enhance the K2 platform by providing the ability to display PDF inline instead of as a file attachment. 

Limitation
1. This control may not work properly in a scenario of network load balance unless sticky session is configured. (This have not been tested)
2. This control currently load the attachment in a folder in the IIS Virtual Directory, a windows scheduler job should be configured to clear it periodically.
3. This control only works with K2's out of the box PDF SmartObject.

The control have been tested with the current K2 version (4.6.11).
