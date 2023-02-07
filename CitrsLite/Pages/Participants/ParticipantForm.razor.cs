using CitrsLite.Business.ViewModels.ParticipantViewModels;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;

namespace CitrsLite.Pages.Participants
{
    public partial class ParticipantForm
    {
        [Inject]
        public ParticipantFormViewModel Model { get; set; }
        private EditContext? editContext { get; set; }
        private bool success;

        public async Task CreateParticipant(EditContext context)
        {
            if (editContext?.Validate() == true)
            {
                participantService.CreateAsync(Model);
                success = true;
                StateHasChanged();
                await clearForm();
            }
            else
            {
                success = false;
            }

        }

        protected override async Task OnInitializedAsync()
        {
            await setUserName();
            editContext = new EditContext(Model);
            base.OnInitialized();
        }

        private async Task clearForm()
        {
            Model = new ParticipantFormViewModel();
            await setUserName();
            editContext = new EditContext(Model);
        }

        private async Task setUserName()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            Model.UserName = authState.User.Identity?.Name ?? "Unknown user";
        }
    }
}
