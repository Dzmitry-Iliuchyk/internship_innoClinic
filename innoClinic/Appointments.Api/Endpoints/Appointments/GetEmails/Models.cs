namespace GetEmails {
    internal sealed class GetEmailsRequest {
        public Guid AppointmentId { get; set; }
    }

    internal sealed class GetEmailsResponse {
        public List<string> Emails { get; set; }
    }
}
