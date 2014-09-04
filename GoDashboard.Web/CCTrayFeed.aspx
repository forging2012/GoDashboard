<%@ Import Namespace="GoDashboard.Web.Modules" %>
<%@ Page Language="c#"%>

<script runat="server">
    public void Page_Load(Object sender, EventArgs e)
    {
        var xDoc = (new DashBoard()).GetPipelinesXmlFromServer();
		
		var xmlOutput = xDoc.OuterXml;

        //var compressedPipelines = (new DashBoard()).CompressPipelineGroup(allPipelines);
    
        //var xmlOutput = (new CCTrayXmlFeed(compressedPipelines)).Output;
        Response.ContentType = "text/xml";
        Response.Write(xmlOutput);
    }
</script>