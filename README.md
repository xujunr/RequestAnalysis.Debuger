1. Build the Project.
2. Copy the RequestAnalysis.Debuger.dll to the Host project bin/debug diretory
3. 
    Web Host: In Web.config system.webServer/modules section, add section `<add name="RequestAnalysisModule" type="RequestAnalysis.Debuger.RequestAnalysisModule, RequestAnalysis.Debuger"/>`

    Self Host: httpServer.Configuration.MessageHandlers.Add(new RequestAnalysisDelegatingHandler())
