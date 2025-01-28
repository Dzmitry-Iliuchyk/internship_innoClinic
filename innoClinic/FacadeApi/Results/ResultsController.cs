using Documents.GrpcApi;
using FacadeApi.Offices.Exceptions;
using FacadeApi.Results.Dtos;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Events.Contracts;
using Shared.PdfGenerator;
using System.IO;
using System.Text.Json;

namespace FacadeApi.ResultsApi {
    [ApiController]
    [Route( "[controller]" )]
    public class ResultsController: ControllerBase {
        private readonly IHttpClientFactory _clientFactory;
        private readonly PdfGeneratorService _pdf;
        private readonly IPublishEndpoint _bus;
        private readonly DocumentService.DocumentServiceClient _documents;

        public ResultsController( IHttpClientFactory clientFactory,
                                  DocumentService.DocumentServiceClient documents,
                                  PdfGeneratorService pdf,
                                  IPublishEndpoint bus ) {
            this._clientFactory = clientFactory;
            this._documents = documents;
            this._pdf = pdf;
            this._bus = bus;
        }
        [HttpPost( "[action]" )]
        public async Task<IResult> CreateResult( ResultCreateRequest r ) {
            using var resultsClient = GetClientWithHeaders();


            var appointmentHttpResult = await resultsClient.GetAsync( $"appointments/{r.AppointmentId}/get" );
            if (!appointmentHttpResult.IsSuccessStatusCode) {
                throw new NotSuccessHttpRequest( appointmentHttpResult );
            }
            var appointment = JsonSerializer.Deserialize<AppointmentResponse>(
                await appointmentHttpResult.Content.ReadAsStreamAsync(),
                options: new() {
                    PropertyNameCaseInsensitive = true
            } );

            var emailsHttpResult = await resultsClient.GetAsync( $"appointments/{appointment.Id}/getEmails" );
            if (!emailsHttpResult.IsSuccessStatusCode) {
                throw new NotSuccessHttpRequest( emailsHttpResult );
            }
            var emails = JsonSerializer.Deserialize<EmailsResponse>( await appointmentHttpResult.Content.ReadAsStreamAsync(),
                options: new() {
                    PropertyNameCaseInsensitive = true
                } );

            var content = JsonContent.Create( new ResultCreateDto {
                AppointmentId = r.AppointmentId,
                Complaints = r.Complaints,
                Conclusion = r.Conclusion,
                DocumentUrl = GetPathToBlob( r.AppointmentId ),
                Recomendations = r.Recomendations
            } );
            var httpResult = await resultsClient.PostAsync( "result/create", content );
            if (!httpResult.IsSuccessStatusCode) {
                throw new NotSuccessHttpRequest( httpResult );
            }

            var pdf = _pdf.GeneratePdf( HtmlTamplates.GetResultsTamplateToPdf( r.Complaints, r.Conclusion, r.Recomendations ) );
   

            var docResult = await _documents.UploadBlobAsync( new BlobUploadRequest() {
                Content = Google.Protobuf.ByteString.CopyFrom( pdf ),
                PathToBlob = GetPathToBlob(r.AppointmentId),
            } );
            await _bus.Publish( new SendEmailRequest() {
                TextContent = HtmlTamplates.GetResultsTamplateForEmailMessage( string.Format( "{0} {1}",
                appointment.PatientFirstName, appointment.PatientSecondName ) ),
                NameFrom = "innoClinic",
                Subject = "Results",
                To = emails.Emails,
                File = new Shared.Events.Contracts.File() {
                    FileName = "result.pdf",
                    FileContentType = "application/pdf",
                    FileContent = pdf
                }
            } );
            return Microsoft.AspNetCore.Http.Results.Content(await httpResult.Content.ReadAsStringAsync(),
                statusCode: (int)httpResult.StatusCode);
        }
        [HttpPut( "[action]" )]
        public async Task<IResult> TestSendEmail( ResultUpdateRequest r ) {
            
            await _bus.Publish( new SendEmailRequest() {
                TextContent = HtmlTamplates.GetResultsTamplateForEmailMessage( string.Format( "{0} {1}", "sfsf", "fsdfs" ) ),
                NameFrom = "innoClinic",
                Subject = "Results update",
                To = [ "dima6061551@mail.ru"],
                
            } );
            return Microsoft.AspNetCore.Http.Results.Ok( );
        }
        [HttpPut( "[action]" )]
        public async Task<IResult> UpdateResult( ResultUpdateRequest r ) {
            using var resultsClient = GetClientWithHeaders();


            var appointment = await resultsClient.GetFromJsonAsync<AppointmentResponse>( $"appointments/{r.AppointmentId}/get" );
            var emails = await resultsClient.GetFromJsonAsync<List<string>>( $"appointments/{appointment.Id}/getEmails" );

            var content = JsonContent.Create( new ResultUpdateDto {
                Id=r.Id,
                AppointmentId = r.AppointmentId,
                Complaints = r.Complaints,
                Conclusion = r.Conclusion,
                DocumentUrl = GetPathToBlob( r.AppointmentId ),
                Recomendations = r.Recomendations
            } );
            var httpResult = await resultsClient.PostAsync( "result/update", content );
            if (!httpResult.IsSuccessStatusCode) {
                throw new NotImplementedException();
            }

            var pdf = _pdf.GeneratePdf( HtmlTamplates.GetResultsTamplateToPdf( r.Complaints, r.Conclusion, r.Recomendations ) );
            
            string pathToBlob = GetPathToBlob( r.AppointmentId );

            await _documents.DeleteBlobAsync( new DeleteBlobRequest() {
                PathToBlob = pathToBlob,
            } );
            var docResult = await _documents.UploadBlobAsync( new BlobUploadRequest() {
                Content = Google.Protobuf.ByteString.CopyFrom( pdf ),
                PathToBlob = pathToBlob,
            } );

            await _bus.Publish( new SendEmailRequest() {
                TextContent = HtmlTamplates.GetResultsTamplateForEmailMessage( string.Format( "{0} {1}",
                appointment.PatientFirstName, appointment.PatientSecondName ) ),
                NameFrom = "innoClinic",
                Subject = "Results update",
                To = emails,
                File = new Shared.Events.Contracts.File() {
                    FileName = "result_update.pdf",
                    FileContentType = "application/pdf",
                    FileContent = pdf
                }
            } );
            return Microsoft.AspNetCore.Http.Results.Ok( await httpResult.Content.ReadAsStringAsync() );
        }

        private static string GetPathToBlob( Guid appointmentId ) {
            return $"results:{appointmentId}/result.pdf";
        }

        private HttpClient GetClientWithHeaders() {
            var resultsClient = _clientFactory.CreateClient( "results" );
            if (Request.Headers.Authorization.Any()) {
                resultsClient.DefaultRequestHeaders.Add( "Authorization", Request.Headers.Authorization.ToString() );
            }
            return resultsClient;
        }
    }
}
