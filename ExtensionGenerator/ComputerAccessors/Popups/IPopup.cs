namespace ExtensionGenerator.Popups;

public interface IPopup
{
    public Task<bool> DisplayToUser();
}