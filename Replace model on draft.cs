using System;
using System.IO;
using System.Runtime.InteropServices;
using SolidEdge.SDK;

namespace ReplaceModelOnDraft
{
class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{		
			SolidEdgeFramework.Application application;
            SolidEdgeFramework.SolidEdgeDocument document;
            SolidEdgeDraft.DraftDocument draftDocument;
            SolidEdgeDraft.ModelLinks modelLinks;
            SolidEdgeDraft.ModelLink modelLink;
            
            try
            { 
                // Connect to Solid Edge
                OleMessageFilter.Register();
                application = (SolidEdgeFramework.Application)Marshal.GetActiveObject("SolidEdge.Application");
                document = (SolidEdgeFramework.SolidEdgeDocument)application.ActiveDocument;
                
                // Check document type
                if (document.Type != SolidEdgeFramework.DocumentTypeConstants.igDraftDocument)
                {
                    return;
                }
                draftDocument = (SolidEdgeDraft.DraftDocument)document;
                modelLinks = draftDocument.ModelLinks;
                if (modelLinks.Count != 1)
                {
                	return;
                }
                modelLink = modelLinks.Item(1);
                string extension = Path.GetExtension(modelLink.FileName);
                string newModelLink = draftDocument.FullName.Replace(".dft", extension);
                if (File.Exists(newModelLink))
                    {
                    	modelLink.ChangeSource(newModelLink);
                		//modelLink.UpdateViews();
                    }
                
            }
               catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
            finally
            {
                Console.WriteLine("Done");
            	Console.ReadKey();
            }
		}
	}
}
