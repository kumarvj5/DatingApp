using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using KaamDatingApp.API.Data;
using KaamDatingApp.API.Dtos;
using KaamDatingApp.API.Helpers;
using KaamDatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KaamDatingApp.API.Controllers
{
    [ServiceFilter(typeof(LoguserActivity))]
    [Route("api/users/{userId}/messages")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDatingRepository _repo;
        public MessagesController(IDatingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name="GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (userId!= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
                
             var message = await _repo.GetMessage(id);
             if (message== null)
                return NotFound();

            return Ok(message);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(int userId,[FromQuery]MessageParams messageParams)
        {
            if (userId!= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
                
            messageParams.UserId = userId;

             var messagesList = await _repo.GetMessagesGorUser(messageParams);
   
            var messagesToReturn = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesList);

            Response.AddPagination(messagesList.CurrentPage, messagesList.PageSize, messagesList.TotalCount, messagesList.TotalPages);

            return Ok(messagesToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDto messageForCreationDto)
        {
            var sender = await _repo.GetUser(userId);

            if (sender.Id!= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
                
             var recipient = await _repo.GetUser(messageForCreationDto.RecipientId);
             if (recipient== null)
                return BadRequest("Could not find User");
            
            var message = _mapper.Map<Message>(messageForCreationDto);
            message.SenderId =userId;
            _repo.Add(message);

            if (await _repo.SaveAll())
            {
                var messageToReturn = _mapper.Map<MessageToReturnDto>(message);
                return CreatedAtRoute("GetMessage", new {id= message.Id}, messageToReturn);
            }
           
            throw new Exception("Error while creating the Message");
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessages(int userId, int recipientId)
        {
            if (userId!= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();         

             var messagesList = await _repo.GetMessageThread(userId, recipientId);
   
            var messagesToReturn = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesList);
            return Ok(messagesToReturn);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int userId, int id)
        {
            if (userId!= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
                
             var message = await _repo.GetMessage(id);
             if (message.SenderId== userId)
                message.SenderDeleted = true;

             if (message.RecipientId== userId)
             message.RecipientDeleted = true;
            
             if (message.SenderDeleted && message.RecipientDeleted)
                _repo.Delete(message);

             if (await _repo.SaveAll()) 
                return NoContent();

            return Ok(message);
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int userId, int id)
        {
            if (userId!= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
                
             var message = await _repo.GetMessage(id);
             if (message.SenderId== userId)
                message.SenderDeleted = true;

             if (message.RecipientId != userId)
                return Unauthorized();
            
             message.IsRead = true;
             message.DateRead = DateTime.Now;

             if (await _repo.SaveAll()) 
                return NoContent();
                
            return NoContent();
        }
    }
}