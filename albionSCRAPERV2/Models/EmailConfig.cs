namespace albionSCRAPERV2.Models;

public class EmailConfig
{
    public string SmtpServer { get; set; } = "";
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public string SenderEmail { get; set; } = "";
    public string SenderPassword { get; set; } = "";

    void cso()
    {
        
    }
}