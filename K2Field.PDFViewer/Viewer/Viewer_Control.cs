using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SourceCode.Forms.Controls.Web.SDK;
using SourceCode.Forms.Controls.Web.SDK.Attributes;

using SourceCode.SmartObjects.Client;
using SourceCode.Hosting.Client;


[assembly: WebResource("K2Field.PDFViewer.Viewer.Viewer_Script.js", "text/javascript", PerformSubstitution = true)]

[assembly: WebResource("K2Field.PDFViewer.Viewer.Viewer_Stylesheet.css", "text/css", PerformSubstitution = true)]

[assembly: WebResource("K2Field.PDFViewer.Viewer.main.js", "text/javascript", PerformSubstitution = false)]
[assembly: WebResource("K2Field.PDFViewer.Viewer.pdf.js", "text/javascript", PerformSubstitution = false)]
[assembly: WebResource("K2Field.PDFViewer.Viewer.pdf.worker.js", "text/javascript", PerformSubstitution = false)]
namespace K2Field.PDFViewer.Viewer
{
    //specifies the location of the embedded definition xml file for the control
    [ControlTypeDefinition("K2Field.PDFViewer.Viewer.Viewer_Definition.xml")]
    [ClientScript("K2Field.PDFViewer.Viewer.Viewer_Script.js")]
    
    [ClientCss("K2Field.PDFViewer.Viewer.Viewer_Stylesheet.css")]

    [ClientScript("K2Field.PDFViewer.Viewer.pdf.js")]
    [ClientScript("K2Field.PDFViewer.Viewer.pdf.worker.js")]
    [ClientScript("K2Field.PDFViewer.Viewer.main.js")]
   

    public class Control : BaseControl
    {
        #region Control Properties
        //to be able to use the control's properties in code-behind, define public properties 
        //with the same names as the properties defined in the Definition.xml file's <Properties> section
        //create get/set methods and return the property of the same name but to lower case

        //in this example, we are exposing the <Prop ID="ControlText"> property from the definition.xml file to the code-behind
        public string ControlText
        {
            get
            {
                return this.Attributes["controltext"];
            }
            set
            {
                this.Attributes["controltext"] = value;
            }
        }

        //IsVisible property
        public bool IsVisible
        {
            get
            {
                return this.GetOption<bool>("isvisible", true);
            }
            set
            {
                this.SetOption<bool>("isvisible", value, true);
            }
        }

        //IsEnabled property
        public bool IsEnabled
        {
            get
            {
                return this.GetOption<bool>("isenabled", true);
            }
            set
            {
                this.SetOption<bool>("isenabled", value, true);
            }
        }

        //Value property. 
        //"Value" is the value set with the standard getValue/getValue js methods. You can override these methods to set a different property
        public string Value
        {
            get { return this.Attributes["value"]; }
            set { this.Attributes["value"] = value; }
        }

        //IsVisible property
        public bool OutputDebugInfo
        {
            get
            {
                return this.GetOption<bool>("outputdebuginfo", true);
            }
            set
            {
                this.SetOption<bool>("outputdebuginfo", value, true);
            }
        }

        #region IDs
        public string ControlID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        public override string ClientID
        {
            get
            {
                return base.ID;
            }
        }

        public override string UniqueID
        {
            get
            {
                return base.ID;
            }
        }
        #endregion

        #endregion

        #region Contructor
        public Control()
             //TODO: if needed, inherit from a HTML type like div or input
        {

        }
        #endregion

        #region Control Methods
        protected override void CreateChildControls()
        {
            base.EnsureChildControls();

            //TODO: if necessary, create child controls for the control.

            //Perform state-specific operations
            switch (base.State)
            {
                case SourceCode.Forms.Controls.Web.Shared.ControlState.Designtime:
                    //assign a temp unique Id for the control
                    this.ID = Guid.NewGuid().ToString();
                    break;
                case SourceCode.Forms.Controls.Web.Shared.ControlState.Preview:
                    //do any Preview-time manipulation here
                    break;
                case SourceCode.Forms.Controls.Web.Shared.ControlState.Runtime:
                    //do any runtime manipulation here
                    this.Attributes.Add("enabled", this.IsEnabled.ToString());
                    this.Attributes.Add("visible", this.IsVisible.ToString());
                    break;
            }

            //if outputting the debug info for the control, add the literal control to the controls collection
            if (OutputDebugInfo)
            {
               
            }

            // Call base implementation last
            base.CreateChildControls();
        }

        protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
        {
            LiteralControl ctrl = new LiteralControl();
            //TODO: if needed, implement a method to render contents
            string strScript = @"
<div>
  <button id='prev'>Previous</button>
  <button id='next'>Next</button>
  &nbsp; &nbsp;
  <span> Page: <span id='page_num'>1</span> / <span id='page_count'>14</span></span>
</div>
<div id='page'>
    <canvas id='canvas'></canvas>
  </div> ";
            ctrl.Text = strScript;
            ctrl.RenderControl(writer);
        }
        #endregion

        /// <summary>
        /// this helper method outputs a label control with various properties for the Smartforms control
        /// it is intended for development and debugging purposes so that you can output the various properties of your custom control
        /// Feel free to add code and properties to the output element
        /// </summary>

        private void getSmartFileFromSMO(string smoName) { 
        
        }

    }

    
}
 