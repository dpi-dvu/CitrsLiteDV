using CitrsLite.Business.ViewModels.ParticipantViewModels;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;

namespace CitrsLite.Pages.Participants
{
    public partial class ParticipantForm
    {
        [Inject]
        public ParticipantFormViewModel Model { get; set; }
        private EditContext editContext { get; set; }
        private bool success;

        public void CreateParticipant(EditContext context)
        {
            if (editContext.Validate() == true)
            {
                participantService.CreateAsync(Model);
                success = true;
                StateHasChanged();
                clearForm();
            }
            else
            {
                success = false;
            }

        }

        protected override async Task OnInitializedAsync()
        {
            setUserName();
            editContext = new EditContext(Model);
            base.OnInitialized();
        }

        private void clearForm()
        {
            Model = new ParticipantFormViewModel();
            setUserName();
            editContext = new EditContext(Model);
        }

        private async void setUserName()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            Model.UserName = authState.User.Identity?.Name ?? "Unknown user";
        }
    }
}
