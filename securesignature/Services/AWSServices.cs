using System;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using PCLStorage;

namespace securesignature.Services
{
    public class AWSServices
    {
		public bool SaveFileToS3(IFile file)
		{
            //Used to authenticate calls to the service
			AWSConfigsS3.UseSignatureVersion4 = true;

			var s3Client = new AmazonS3Client("AKIAJZJBX6CIURKKV6UA", 
                                              "wFcc9HLPkhmn5bzu0tFP23lkISeWFhatU+MrHJ6j", 
                                              Amazon.RegionEndpoint.USWest2);
        
			var transferUtility = new TransferUtility(s3Client);

            try{

                System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken();

                TransferUtilityUploadRequest req = new TransferUtilityUploadRequest();
                req.FilePath = file.Path;
                req.BucketName = "android-signatures";
                req.Key = file.Name;

                transferUtility.UploadAsync(req, cancellationToken);

                //if(.IsCompleted){
                //    System.Diagnostics.Debug.WriteLine("File upload completed.");
                //    System.Diagnostics.Debug.WriteLine("Is Faulty Completion: " +
                //                                       $"{transferUtility.UploadAsync(req,cancellationToken).IsFaulted}");
                //}

                return true;
            }
            catch(Exception){
                //System.Diagnostics.Debug.WriteLine($"Upload Exception: {e}");
                return false;
            }
		}
    }
}
