using System;
using System.Runtime.InteropServices;
using SolidEdge.SDK;

namespace Steering_Wheel_by_GCS
{
	class Program
	{		
		[STAThread]
		public static void Main(string[] args)
		{
			SolidEdgeFramework.Application application;
            SolidEdgeFramework.SolidEdgeDocument document;
            SolidEdgeFramework.Window window;
            SolidEdgeFramework.View view;
            
            try
            { 
                // Connect to Solid Edge
                OleMessageFilter.Register();
                application = (SolidEdgeFramework.Application)Marshal.GetActiveObject("SolidEdge.Application");
                document = (SolidEdgeFramework.SolidEdgeDocument)application.ActiveDocument;
                
                if (document.Type == SolidEdgeFramework.DocumentTypeConstants.igDraftDocument) {
                	return;
                }
                
                window = (SolidEdgeFramework.Window)application.ActiveWindow;
                view = window.View;
                
            	          
                switch (document.Type)
                {
                	case SolidEdgeFramework.DocumentTypeConstants.igPartDocument:
                	case SolidEdgeFramework.DocumentTypeConstants.igSheetMetalDocument:
                		var partDocument = (SolidEdgePart.PartDocument)document;
                		//OrientSteeringWheelByView(view, partDocument.SteeringWheel);
                		OrientSteeringWheelByGCS(partDocument.SteeringWheel);
                		break;
                	case SolidEdgeFramework.DocumentTypeConstants.igAssemblyDocument:
                		var assemblyDocument = (SolidEdgeAssembly.AssemblyDocument)document;
                		//OrientSteeringWheelByView(view, assemblyDocument.SteeringWheel);
                		OrientSteeringWheelByGCS(assemblyDocument.SteeringWheel);
                		break;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
            finally
            {
            	application = null;
            	document = null;
            	OleMessageFilter.Revoke();
            }
		}
		
		static void OrientSteeringWheelByView(SolidEdgeFramework.View view, SolidEdgeFramework.SteeringWheel steeringWheel){
			double eyeX, eyeY, eyeZ, targetX, targetY, targetZ, upX, upY, upZ, scaleOrAngle;
			bool perspective;
			view.GetCamera(out eyeX, out eyeY, out eyeZ, out targetX, out targetY, out targetZ, out upX, out upY, out upZ, out perspective, out scaleOrAngle);
			steeringWheel.Align(SolidEdgeFramework.seSteeringWheelConstants.seSteeringWheelConstantsZAxis, eyeX - targetX, eyeY - targetY, eyeZ - targetZ);
			steeringWheel.Align(SolidEdgeFramework.seSteeringWheelConstants.seSteeringWheelConstantsYAxis, upX, upY, upZ);
		}
		
		static void OrientSteeringWheelByGCS(SolidEdgeFramework.SteeringWheel steeringWheel){
			steeringWheel.Align(SolidEdgeFramework.seSteeringWheelConstants.seSteeringWheelConstantsZAxis, 0, 0, 1);
			steeringWheel.Align(SolidEdgeFramework.seSteeringWheelConstants.seSteeringWheelConstantsYAxis, 0, 1, 0);
		}
	}
}
