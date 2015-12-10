//NOTE: alert() statements are available for debugging purposes. You can uncomment the statements to show dialogs when each method is hit.
(function ($) {
    //TODO: if necessary, add additional statements to initialize each part of the namespace before your Viewer is called. 
    if (typeof K2Field === "undefined" || K2Field == null) K2Field = {};
    if (typeof K2Field.PDFViewer === "undefined" || K2Field.PDFViewer == null) K2Field.PDFViewer = {};
   // if (typeof K2Field.PDFViewer.Viewer === "undefined" || K2Field.PDFViewer.Viewer == null) K2Field.PDFViewer.Viewer = {};
    var tempURL;
    K2Field.PDFViewer.Viewer = {

        //internal method used to get a handle on the control instance
        _getInstance: function (id) {
            //alert("_getInstance(" + id + ")");
            var control = jQuery('#' + id);
            if (control.length == 0) {
                throw 'Viewer \'' + id + '\' not found';
            } else {
                return control[0];
            }
        },

        getValue: function (objInfo) {
            alert("getValue() for control " + objInfo.CurrentControlId);
            var instance = K2Field.PDFViewer.Viewer._getInstance(objInfo.CurrentControlId);
            return instance.value;
        },

        getDefaultValue: function (objInfo) {
            //alert("getDefaultValue() for control " + objInfo.CurrentControlId);
            getValue(objInfo);
        },

        setValue: function (objInfo) {
            //alert("setValue() for control " + objInfo.CurrentControlId);
            var instance = K2Field.PDFViewer.Viewer._getInstance(objInfo.CurrentControlId);
            var oldValue = instance.value;
            //only change the value if it has actually changed, and then raise the OnChange event
            if (oldValue != objInfo.Value) {
                instance.value = objInfo.Value;
                raiseEvent(objInfo.CurrentControlId, 'Control', 'OnChange');
            }
            var filetemp;
            $(function () {
                var controlid = document.getElementsByClassName('SFC K2Field-PDFViewer-Viewer-Control')[0].id;
                var control = K2Field.PDFViewer.Viewer._getInstance(controlid).value;

                $.ajax(
                        {
                             type: 'POST',
                             url: 'K2Field.PDFViewer/GetFileFromIsolatedStorage.handler',
                             cache: false,
                             data: { control: control },
                             async:false
                         }).done(function (file) {
                             tempURL = file;   
                         });
            });
        },

        //retrieve a property for the control
        getProperty: function (objInfo) {
           
            if (objInfo.property.toLowerCase() == "value") {
                return K2Field.PDFViewer.Viewer.getValue(objInfo);
            }
            else {
                return $('#' + objInfo.CurrentControlId).data(objInfo.property);
            }
        },

        //set a property for the control. note case statement to call helper methods
        setProperty: function (objInfo) {
            switch (objInfo.property.toLowerCase()) {
                case "style":
                    K2Field.PDFViewer.Viewer.setStyles(null, objInfo.Value, $('#' + objInfo.CurrentControlId));
                    break;
                case "value":
                    K2Field.PDFViewer.Viewer.setValue(objInfo);
                    break;
                case "isvisible":
                    K2Field.PDFViewer.Viewer.setIsVisible(objInfo);
                    break;
                case "isenabled":
                    K2Field.PDFViewer.Viewer.setIsEnabled(objInfo);
                    break;
                default:
                    $('#' + objInfo.CurrentControlId).data(objInfo.property).value = objInfo.Value;
            }
        },

        validate: function (objInfo) {
            //alert("validate for control " + objInfo.CurrentControlId);
        },

        //helper method to set visibility
        setIsVisible: function (objInfo) {
            //alert("set_isVisible: " + objInfo.Value);
            value = (objInfo.Value === true || objInfo.Value == 'true');
            this._isVisible = value;
            var displayValue = (value === false) ? "none" : "block";
            var instance = K2Field.PDFViewer.Viewer._getInstance(objInfo.CurrentControlId);
            instance.style.display = displayValue;
        },

        //helper method to set control "enabled" state
        setIsEnabled: function (objInfo) {
            //alert("set_isEnabled: " + objInfo.Value);
            value = (objInfo.Value === true || objInfo.Value == 'true');
            this._isEnabled = value;
            var instance = K2Field.PDFViewer.Viewer._getInstance(objInfo.CurrentControlId);
            instance.readOnly = !value;
        },

        setStyles: function (wrapper, styles, target) {
            var isRuntime = (wrapper == null);
            var options = {};
            var element = isRuntime ? jQuery(target) : wrapper.find('.K2Field.PDFViewer.Viewer');

            jQuery.extend(options, {
                "border": element,
                "background": element,
                "margin": element,
                "padding": element,
                "font": element,
                "horizontalAlign": element
            });

            StyleHelper.setStyles(options, styles);
        },
        execute: function (objInfo) {
            var parameters = objInfo.methodParameters;
            var method = objInfo.methodName;
            var result = "";
            var currentControlID = objInfo.CurrentControlID;
            switch (method) {
                case "displayPDF":
                    //var pdfAsDataUri = "data:application/pdf;base64," + tempvalue;
                    //alert("This is tempvalue : " + tempvalue);
                    //alert("This is pdfAsDataURI : " + pdfAsDataUri);
                    //var encodedString = Base64.encode(pdfAsDataUri);
                    ////console.log(encodedString); // Outputs: "SGVsbG8gV29ybGQh"
                    //alert("Encoded String : " + encodedString);
                    //// Decode the String
                    //var decodedString = Base64.decode(encodedString);
                    //console.log(decodedString);
                   // alert("Temp URL VALUE : " + tempURL);
                    PDFJS.getDocument(tempURL).then(function (pdf) {
                        pdfFile = pdf;
                     //   alert("Num of pages : " + pdfFile.numPages);
                        pdfFile.crossOrigin = ' ';
                        document.getElementById('page_count').textContent = pdfFile.numPages;

                        pageNum = pdfFile.numPages;
                        //alert("This is pageNum :" +pageNum);
                        //alert("This is currPageNumber : " + currPageNumber);
                        openPage(pdf, currPageNumber, 1);
                        // renderPage(pageNum);
                    });
                    //alert('done with execute function');
                    break;
                    //console.log('done');
            }
        }
    };
})(jQuery);

$(document).ready(function () {

    //add a delegate event handler for user-driven clicks 
    //TODO: add events for other user-driven events. 
    //(Note that custom controls created with the SDK have .SFC as the class)
    //you could also use event binding, if preferred 

    $(document).delegate('.SFC.K2Field.PDFViewer-Viewer-Control', 'click.Control', function (e) {
        //alert("control " + this.id + " clicked");
        raiseEvent(this.id, 'Control', 'OnClick');
    });

    $(document).delegate(".SFC.K2Field.PDFViewer-Viewer-Control", "change.Control", function (e) {
        //alert("control " + this.id + " changed");
        raiseEvent(this.id, 'Control', 'OnChange');
    });
});