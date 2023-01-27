using CitrsLite.Business.ViewModels.ParticipantViewModels;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;

namespace CitrsLite.Pages.Participants
{
    public partial class ParticipantForm
    {
        [Inject]
        public ParticipantFormViewModel Model { get; set; }
        private EditContext EC { get; set; }
        private bool success;

        public void CreateParticipant(EditContext context)
        {
            if (EC.Validate() == true)
            {
                participantService.CreateAsync(Model);
                success = true;
                StateHasChanged();
                ClearForm();
            }
            else
            {
                success = false;
            }

        }

        protected override async Task OnInitializedAsync()
        {
            setUserName();
            EC = new EditContext(Model);
            base.OnInitialized();
        }

        private void ClearForm()
        {
            Model = new ParticipantFormViewModel();
            setUserName();
            EC = new EditContext(Model);
        }

        private async void setUserName()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            Model.UserName = authState.User.Identity?.Name ?? "Unknown user";
        }
    }
}
