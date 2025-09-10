using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using myApi.CustomAcionFilters;
using myApi.Data;
using myApi.DTO;
using myApi.Models.Domain;
using myApi.Repositries;
using System.Diagnostics.Eventing.Reader;
using System.Text.Json;

namespace myApi.Controllers
{
    //localhost:portnumber/api/regions
    [Route("api/[controller]")]
    [ApiController]

    public class RegionsController : ControllerBase
    {
        private readonly MyApiDbContext dbcontext;
        private readonly IRegionRepositry regionReposirty;
        private readonly IMapper mapper;

        private readonly ILogger<RegionsController> logger;

        public RegionsController(MyApiDbContext dbcontext, IRegionRepositry regionReposirty, IMapper mapper, ILogger<RegionsController> logger)
        {
            this.dbcontext = dbcontext;
            this.regionReposirty = regionReposirty;
            this.mapper = mapper;
            this.logger = logger;

        }
        //Get All Regions
        // Get:localhost:portnumber/api/regions
        [HttpGet]
        // [Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAll()
        {
            //Fetch all regions from the database
            var regions = await regionReposirty.GetAllAsync();
            logger.LogInformation($"Get all regions from database{JsonSerializer.Serialize(regions)}");
            /* 
            //Map Domain models to dto objects
            var regionsDto = new List<RegionDto>();
            foreach (var region in regions)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl

                });
            }
            */
            //Using AutoMapper to map domain models to dto objects
            var regionsDto = mapper.Map<List<RegionDto>>(regions);

            //Return the dto objects to the client
            return Ok(regionsDto);

        }
        //Get single region by id
        //Get:localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
       // [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            //Get the region by id from the database
            var region = await regionReposirty.GetByIdAsync(id);
            if (region == null) {
                return NotFound();
            }
            //Map Domain model to dto object
            /* var regionsDto=new List<RegionDto>();
              regionsDto.Add(new RegionDto()
              {
                  Id = region.Id,
                  Code = region.Code,
                  Name = region.Name,
                  RegionImageUrl = region.RegionImageUrl
              });*/
            //Using AutoMapper to map domain model to dto object
            var regionsDto = mapper.Map<RegionDto>(region);
            //return the dto object to the client
            return Ok(regionsDto);
        }
        //Add a new region
        //Post:localhost:portnumber/api/regions
        [HttpPost]
        [ValidateModel]
       // [Authorize(Roles = "Writer")]

        public async Task<IActionResult> create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
           

                    //Map domain model to dto
                    /*var region = new Region()
                    {
                        Code = addRegionRequestDto.Code,
                        Name=addRegionRequestDto.Name,
                        RegionImageUrl=addRegionRequestDto.RegionImageUrl
                    };*/
                    //Using AutoMapper to map dto to domain model
                    var region = mapper.Map<Region>(addRegionRequestDto);
                    //use domain model to create region
                    region = await regionReposirty.CreateAsync(region);

                    //Map domain model to dto
                    /* var regionDto = new RegionDto()
                     {
                         Id = region.Id,
                         Code = region.Code,
                         Name = region.Name,
                         RegionImageUrl = region.RegionImageUrl
                     };*/
                    //Using AutoMapper to map domain model to dto
                    var regionDto = mapper.Map<RegionDto>(region);
                    return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
                }
              
            
         
        
       
        //update an existing region
        //Put:localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]

        public  async Task<IActionResult> update([FromRoute] Guid id,[FromBody] UpdateRegionRequestDto updateRegionRequestDto) {
            
                //check if the region exists
                //Map dto to domain model
                /* var regionDomainModel = new Region()
                 {
                     Code = updateRegionRequestDto.Code,
                     Name = updateRegionRequestDto.Name,
                     RegionImageUrl = updateRegionRequestDto.RegionImageUrl
                 };*/
                //Using AutoMapper to map dto to domain model
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
                //use domain model to update region
                await regionReposirty.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }
                //Map domain model to dto
                /* var regionDto = new RegionDto()
                 {
                     Id = regionDomainModel.Id,
                     Code = regionDomainModel.Code,
                     Name = regionDomainModel.Name,
                     RegionImageUrl = regionDomainModel.RegionImageUrl
                 };*/
                //Using AutoMapper to map domain model to dto
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);
                return Ok(regionDto);
        
           
        }
        //Delete an existing region
        //Delete:localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
       // [Authorize(Roles = "Writer")]
        public async Task<IActionResult> delete([FromRoute] Guid id) {
         
            //Delete the region
          var regionDomainModels= await regionReposirty.DeleteAsync(id);
            //Return no content
            if (regionDomainModels == null)
            {
                return NotFound();
            }
            //Map domain model to dto
            /*var regionDto = new RegionDto()
            {
                Id = regionDomainModels.Id,
                Code = regionDomainModels.Code,
                Name = regionDomainModels.Name,
                RegionImageUrl = regionDomainModels.RegionImageUrl
            };*/
            //Using AutoMapper to map domain model to dto
            var regionDto = mapper.Map<RegionDto>(regionDomainModels);
            return Ok(regionDto);
        }
    }
}
