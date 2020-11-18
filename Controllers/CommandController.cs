using System.Collections.Generic;
using AutoMapper;
using Commander.Dtos;
using Commander.Models;
using Commander.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Controllers
{
    [Route("api/commands")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepo _repository;

        private readonly IMapper _mapper;
        // private readonly MockCommanderRepo _repository = new MockCommanderRepo();
        public CommandsController(ICommanderRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //Get api/commands
        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = _repository.GetAllCommands();
            // return Ok(commandItems);
            // mapped dto type
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }
        // Get api/commands/id
        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            // returns the DTO instead now
            Command commandItem = _repository.GetCommandById(id);
            if (commandItem != null)
            {
                // and map the return into the mapped type
                return Ok(_mapper.Map<CommandReadDto>(commandItem));
            }
            // 204 No Content error code if request for ID that doesn't exist
            // return Ok(commandItem);
            // content not found if ID doesn't exist
            return NotFound();
        }
        
        //Post api/commands
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            // map incoming to our command to send to create 
            Command commandModel = _mapper.Map<Command>(commandCreateDto);
            
            // use the repo to create the command
            _repository.CreateCommand(commandModel);
            
            // actually persist the changes
            _repository.SaveChanges();

            // map the command model back to the return type now
            // the commandModel is a reference type!!!!!!!!!!!!
            CommandReadDto commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            // to use the same command created on the api already you can request
            // info 
            // returns 201 created
            // and adds the location of the object in the header! as location:
            return CreatedAtRoute(
                nameof(GetCommandById),
                new {id = commandReadDto.Id},
                commandReadDto
                );
            // return Ok(commandReadDto);
        }
    }
}