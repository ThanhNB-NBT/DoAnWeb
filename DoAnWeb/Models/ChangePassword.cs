namespace DoAnWeb.Models
{
    public class ChangePassword
    {
        public int AccountId { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }

}
