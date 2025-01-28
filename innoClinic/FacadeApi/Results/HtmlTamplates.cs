namespace FacadeApi.ResultsApi {
    public static class HtmlTamplates {
        public static string GetResultsTamplateToPdf( string complaints, string conclusion, string? recomendations ) {
            return $@"
            <!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Medical Report</title>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        margin: 20px;
                    }}
                    h1 {{
                        text-align: center;
                        font-size: 24px;
                    }}
                    .report-section {{
                        border: 1px solid #cccccc;
                        padding: 20px;
                        margin: 20px 0;
                    }}
                    .report-section h2 {{
                        font-size: 20px;
                        margin-bottom: 10px;
                    }}
                    .report-section p {{
                        font-size: 16px;
                    }}
                </style>
            </head>
            <body>

            <h1>Medical Report</h1>

            <div class=""report-section"">
                <h2>Complaints</h2>
                <p>{complaints}</p>
            </div>

            <div class=""report-section"">
                <h2>Conclusion</h2>
                <p>{conclusion }</p>
            </div>

            <div class=""report-section"">
                <h2>Recomendations</h2>
                <p>{recomendations ?? "No recomendations provided"}</p>
            </div>

            </body>
            </html>
            ";
        }
        public static string GetResultsTamplateForEmailMessage( string name ) {
            return $@"  <!DOCTYPE html>
                        <html lang=""en"">
                        <head>
                            <meta charset=""UTF-8"">
                            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                            <title>Email with results </title>
                            <style>
                                body {{
                                    font-family: Arial, sans-serif;
                                    margin: 20px;
                                    line-height: 1.6;
                                }}
                                h1 {{
                                    text-align: center;
                                    font-size: 24px;
                                }}
                                .email-content {{
                                    border: 1px solid #cccccc;
                                    padding: 20px;
                                    margin: 20px 0;
                                }}
                                .email-content h2 {{
                                    font-size: 20px;
                                    margin-bottom: 10px;
                                }}
                                .email-content p {{
                                    font-size: 16px;
                                    margin-bottom: 10px;
                                }}
                            </style>
                        </head>
                        <body>

                        <h1>Email with Results File</h1>

                        <div class=""email-content"">
                            <h2>Dear {name},</h2>
                            <p>We are pleased to inform you that your results are ready. Please find the attached file with your results below.</p>
    
                            <p>Thank you for your patience.</p>
    
                            <p>Best regards,<br> innoClinic </p>
    
                        </div>

                        </body>
                        </html>
                        ";
        }
    }
}
