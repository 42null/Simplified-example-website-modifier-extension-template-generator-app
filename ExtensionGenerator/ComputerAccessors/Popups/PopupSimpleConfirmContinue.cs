namespace ExtensionGenerator.Popups;

public class PopupSimpleOkConfirm : IPopup
{
    public const string ACCEPT = "OK";
    public const string CANCEL = "Cancel";
    public const string TITLE_START = "Confirm";
    private string _titleAfter;
    public string TitleAfter
    {
        get { return TITLE_START+_titleAfter; }
        set { _titleAfter = value; }
    }
    public string Message { get; set; }

    public PopupSimpleOkConfirm(string message, string titleAfterConfirm = "")
    {
        Message = message;
        _titleAfter = titleAfterConfirm;
    }

    public async Task<bool> DisplayToUser()
    {
        return await Application.Current.MainPage.DisplayAlert(TITLE_START+" "+_titleAfter, Message, ACCEPT, CANCEL);
    }
}