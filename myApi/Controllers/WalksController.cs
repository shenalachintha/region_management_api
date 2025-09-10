using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myApi.DTO;
using myApi.Models.Domain;
using myApi.Repositries;

namespace myApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepositry walkRepositry;
        public WalksController(IMapper mapper, IWalkRepositry walkRepositry)
        {
            this.mapper = mapper;
            this.walkRepositry = walkRepositry;
        }
        [HttpPost]
        [CustomAcionFilters.ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalksRequestDto addWalksRequestDto) { 
       
                //Convert dto to domain model
                var walkDomainModel = mapper.Map<Walks>(addWalksRequestDto);
                //Pass details to repositry
                walkDomainModel = await walkRepositry.createAsync(walkDomainModel);
                //Convert domain model to dto
                var walksDto = mapper.Map<WalksDto>(walkDomainModel);
                //Return the dto object to the client
                return CreatedAtAction(nameof(GetById), new { id = walksDto.Id }, walksDto);
           
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQurey,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000
            ) {
            var walksDomainModel = await walkRepositry.getAllAsync(filterOn,filterQurey,sortBy,isAscending ?? true,pageNumber,pageSize);
            throw new Exception("This is new exception");
            //map to dto
            return Ok(mapper.Map<List<WalksDto>>(walksDomainModel));
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            var walksDomainModel = await walkRepositry.getByIdAsync(id);
            if (walksDomainModel == null)
            {
                return NotFound();
            }
            //map to dto
            return Ok(mapper.Map<WalksDto>(walksDomainModel));
        }
        [HttpPut]
        [Route("{id:Guid}")]
        [CustomAcionFilters.ValidateModel]
        public async Task<IActionResult> UpdateById([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
          
                //Convert dto to domain model
                var walkDomainModel = mapper.Map<Walks>(updateWalkRequestDto);
                //Pass details to repositry
                walkDomainModel = await walkRepositry.updateByIdAsync(id, walkDomainModel);
                if (walkDomainModel == null)
                {
                    return NotFound();
                }
                //Convert domain model to dto
                var walksDto = mapper.Map<WalksDto>(walkDomainModel);
                //Return the dto object to the client
                return Ok(walksDto);
            
           
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) { 
            var WalkDomainModel=await walkRepositry.deleteByIdAsync(id);
            if (WalkDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalksDto>(WalkDomainModel));

        }

    }
  
}
