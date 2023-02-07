using CitrsLite.Business.ViewModels.ParticipantViewModels;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;

namespace CitrsLite.Pages.Participants
{
    public partial class EditParticipant
    {
        [Inject]
        public ParticipantFormViewModel Model { get; set; }
        [Parameter]
        public int Id { get; set; }
        private EditContext? editContext { get; set; }
        private bool success;

        public async Task UpdateParticipant(EditContext context)
        {
            if (editContext?.Validate() == true)
            {
                await setUserName();
                participantService.Update(Model, Model.UserName);
                success = true;
                StateHasChanged();
            }
            else
            {
                success = false;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await setUserName(); 
            
            ParticipantDetailViewModel part = participantService.GetParticipant(Id);
            Model.Id = part.Id;
            Model.Name = part.Name;
            Model.Type = part.Type;
            Model.Description = part.Description;
            Model.PhoneNumber = part.PhoneNumber;
            Model.Address = part.Address;
            Model.City = part.City;
            Model.State = part.State;
            
            editContext = new EditContext(Model);

            base.OnInitialized();
        }

        private async Task resetForm()
        {
            ParticipantDetailViewModel part = participantService.GetParticipant(Id);
            Model.Name = part.Name;
            Model.Type= part.Type;
            Model.Description = part.Description;
            Model.PhoneNumber= part.PhoneNumber;
            Model.Address= part.Address;
            Model.City= part.City;
            Model.State= part.State;

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
