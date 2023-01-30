﻿using CitrsLite.Business.Repositories;
using CitrsLite.Business.ViewModels.ParticipantViewModels;
using CitrsLite.Data.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitrsLite.Business.Services
{
    public class ParticipantService
    {
        private IUnitOfWork _data;

        public ParticipantService(string connectionString)
        {
            _data= new UnitOfWork(connectionString);
        }

        public Participant GetParticipant(ParticipantFormViewModel formModel)
        {
            return new Participant()
            {
                Name = formModel.Name,
                Type = formModel.Type,
                Description = formModel.Description,
                PhoneNumber = formModel.PhoneNumber,
                Address = formModel.Address,
                City = formModel.City,
                State = formModel.State,
                IsActive = true,
                CreatedBy = formModel.UserName,
                CreationDate = DateTime.Now,
                ModifiedBy = formModel.UserName,
            };
        }

        public ParticipantDetailViewModel GetParticipant(int id)
        {
            Participant part = _data.Participants.GetFirst(p => p.Id == id);

            return new ParticipantDetailViewModel()
            {
                Name = part.Name,
                Type = part.Type,
                Description = part.Description ?? "No description given",
                PhoneNumber = part.PhoneNumber,
                Address = part.Address,
                City = part.City,
                State = part.State,
                IsActive = part.IsActive
            };
        }

        public void Create(ParticipantFormViewModel formModel)
        {
            Participant participant = GetParticipant(formModel);

            _data.Participants.Create(participant);
            _data.SaveChanges();
            
        }

        public async void CreateAsync(ParticipantFormViewModel formModel)
        {
            Participant participant = GetParticipant(formModel);

            await _data.Participants.CreateAsync(participant);

            await _data.SaveChangesAsync();
        }

        public IEnumerable<Participant> GetList()
        {
            return _data.Participants.GetList();
        }

        public async Task<IEnumerable<Participant>> GetListAsync()
        { 
            return await _data.Participants.GetListAsync();
        }

        public void Update(ParticipantFormViewModel model, string userName)
        {
            try
            {
                Participant participant = _data.Participants.GetFirst(p => p.Id == model.Id);

                participant.Name = model.Name;
                participant.Type = model.Type;
                participant.Address = model.Address;
                participant.PhoneNumber = model.PhoneNumber;
                participant.City = model.City;
                participant.State = model.State;
                participant.Description= model.Description;

                participant.ModificationDate = DateTime.Now;
                participant.ModifiedBy = userName;

                _data.Participants.Edit(participant);
                _data.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update Participant Error");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
