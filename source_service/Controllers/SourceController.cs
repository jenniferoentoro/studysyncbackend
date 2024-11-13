using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using source_service.Dtos.Category;
using source_service.Dtos.Source;
using source_service.Dtos.User;
using source_service.Model;
using source_service.RabbitMQ;
using source_service.Service.Interface;
using source_service.Dtos.Error;
using System.Diagnostics;
using source_service.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace source_service.Controllers
{
    [Route("api/source")]
    [ApiController]
    public class SourceController : ControllerBase
    {

        private readonly ICacheService _cacheService;
        private readonly ISourceService _sourceService;

        private readonly ICategoryService _categoryService;


        private readonly IMapper _mapper;

        private readonly IFileService _fileService;

        private readonly IFileStorageService _fileStorageService;


        public SourceController(ISourceService sourceService, ICategoryService categoryService, IMapper mapper, IFileService fileService, ICacheService cacheService, IFileStorageService fileStorageService)
        {
            _sourceService = sourceService;
            _categoryService = categoryService;
            _mapper = mapper;
            _fileService = fileService;
            _cacheService = cacheService;
            _fileStorageService = fileStorageService;

        }
        [HttpGet]
        public async Task<ActionResult> GetSources([FromQuery] QueryObjectSource query)
        {
            var cacheKey = $"{query.Title}{query.UserId}{query.SortBy}{query.IsDecsending}{query.PageNumber}{query.PageSize}";

            var cacheData = _cacheService.GetData<SourceResponseDto>(cacheKey);
            if (cacheData != null)
            {
                return Ok(cacheData);
            }

            bool userServiceAvailable = IsUserServiceAvailable().Result;

            Console.WriteLine("User service available: " + userServiceAvailable);
            if (!userServiceAvailable)
            {
                return StatusCode(503, new CustomErrorResponse
                {
                    Type = "Service Unavailable",
                    Title = "One or more errors occurred.",
                    Status = 503,
                    Message = "User service is currently unavailable.",
                    TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }

            var sources = await _sourceService.GetSources(query);
            List<SourceDto> sourcesDto = new List<SourceDto>();
            foreach (var source in sources)
            {
                var categoryActionResult = await _categoryService.GetCategoryById(source.CategoryId);
                if (categoryActionResult == null)
                {
                    return NotFound();
                }
                var category = (Category)categoryActionResult;

                UserDto userDetails = null;

                var rpcClient = new RpcClient("users");
                Console.WriteLine(" [x] Requesting user details");
                var response = await rpcClient.Call(source.UserId.ToString());
                userDetails = JsonSerializer.Deserialize<UserDto>(response);
                Console.WriteLine(" [.] Got '{0}'", response);

                var sourceDto = _mapper.Map<SourceDto>(source);
                sourceDto.Category = _mapper.Map<CategoryDto>(category);
                sourceDto.User = userDetails;
                Console.WriteLine("User details: " + userDetails);

                sourcesDto.Add(sourceDto);
            }



            var totalCount = await _sourceService.CountSources();


            int currentPage = query.PageNumber > 0 ? query.PageNumber : 1;
            int totalPages = (int)Math.Ceiling((double)totalCount / query.PageSize);
            SourceResponseDto sourceResponseDto = new SourceResponseDto
            {
                Total = totalCount,
                CurrentPage = currentPage,
                TotalPages = totalPages,
                Sources = sourcesDto
            };

            cacheData = sourceResponseDto;

            var expiryTime = DateTimeOffset.Now.AddSeconds(120);

            _cacheService.SetData<SourceResponseDto>(cacheKey, cacheData, expiryTime);
            return Ok(sourceResponseDto);
        }


        [HttpGet("user")]

        public async Task<ActionResult> GetSourcesByUserId()
        {
            var requestUserId = User.FindFirstValue("appid");
            Console.WriteLine("Request user ID: " + requestUserId);
            if (string.IsNullOrEmpty(requestUserId))
            {
                return Unauthorized(new { message = "User ID from the token is missing or invalid." });
            }
            var cacheData = _cacheService.GetData<IEnumerable<SourceDto>>($"sourcesuser{requestUserId}");



            if (cacheData != null && cacheData.Any())
            {
                return Ok(cacheData);
            }

            bool userServiceAvailable = await IsUserServiceAvailable();
            Console.WriteLine("User service available: " + userServiceAvailable);
            if (!userServiceAvailable)
            {
                return StatusCode(503, new CustomErrorResponse
                {
                    Type = "Service Unavailable",
                    Title = "One or more errors occurred.",
                    Status = 503,
                    Message = "User service is currently unavailable.",
                    TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }

            var sources = await _sourceService.GetSourcesByUserId(requestUserId);
            Console.WriteLine("Sources: " + sources);
            List<SourceDto> sourcesDto = new List<SourceDto>();
            foreach (var source in sources)
            {
                var categoryActionResult = await _categoryService.GetCategoryById(source.CategoryId);
                if (categoryActionResult == null)
                {
                    return NotFound();
                }


                var category = (Category)categoryActionResult;

                UserDto userDetails = null;

                var rpcClient = new RpcClient("users");
                Console.WriteLine(" [x] Requesting user details");
                var response = await rpcClient.Call(source.UserId.ToString());
                userDetails = JsonSerializer.Deserialize<UserDto>(response);
                Console.WriteLine(" [.] Got '{0}'", response);

                var sourceDto = _mapper.Map<SourceDto>(source);
                sourceDto.Category = _mapper.Map<CategoryDto>(category);
                sourceDto.User = userDetails; // UserDetails will be null if user service is down
                if (source.privacy == "private")
                {
                    var fileStorage = await _fileStorageService.GetFileStorageBySourceId(source.Id);
                    sourceDto.UrlFile = fileStorage.Token;
                }
                Console.WriteLine("User details: " + userDetails);

                sourcesDto.Add(sourceDto);
            }

            cacheData = sourcesDto;

            var expiryTime = DateTimeOffset.Now.AddSeconds(120);

            _cacheService.SetData<IEnumerable<SourceDto>>($"sourcesuser{requestUserId}", cacheData, expiryTime);

            var totalCount = await _sourceService.CountSources();


            return Ok(sourcesDto);

        }



        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult> GetSource(string id)
        {
            var cacheData = _cacheService.GetData<SourceDto>($"source{id}");

            if (cacheData != null)
            {
                Console.WriteLine("Cache hit");
                return Ok(cacheData);
            }

            bool userServiceAvailable = await IsUserServiceAvailable();
            Console.WriteLine("User service available: " + userServiceAvailable);
            if (!userServiceAvailable)
            {
                return StatusCode(503, new CustomErrorResponse
                {
                    Type = "Service Unavailable",
                    Title = "One or more errors occurred.",
                    Status = 503,
                    Message = "User service is currently unavailable.",
                    TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }

            var source = await _sourceService.GetSource(id);
            if (source == null)
            {
                return NotFound();
            }

            var requestUserId = User.FindFirstValue("appid");
            if (string.IsNullOrEmpty(requestUserId))
            {
                return Unauthorized(new { message = "User ID from the token is missing or invalid." });
            }

            if (source.privacy == "private" && source.UserId != requestUserId)
            {
                return Forbid();
            }

            var categoryActionResult = await _categoryService.GetCategoryById(source.CategoryId);
            if (categoryActionResult == null)
            {
                return NotFound();
            }
            var category = (Category)categoryActionResult;

            UserDto userDetails = null;

            var rpcClient = new RpcClient("users");
            Console.WriteLine(" [x] Requesting user details");
            var response = await rpcClient.Call(source.UserId.ToString());
            userDetails = JsonSerializer.Deserialize<UserDto>(response);
            Console.WriteLine(" [.] Got '{0}'", response);

            var sourceDto = _mapper.Map<SourceDto>(source);
            sourceDto.Category = _mapper.Map<CategoryDto>(category);
            sourceDto.User = userDetails;
            Console.WriteLine("User details: " + userDetails);

            if (source.privacy == "private")
            {
                var fileStorage = await _fileStorageService.GetFileStorageBySourceId(source.Id);
                sourceDto.UrlFile = fileStorage.Token;
            }

            cacheData = sourceDto;

            var expiryTime = DateTimeOffset.Now.AddSeconds(120);

            _cacheService.SetData<SourceDto>($"source{id}", cacheData, expiryTime);

            return Ok(sourceDto);
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddSource([FromForm] CreateSourceDto source)
        {

            var requestUserId = User.FindFirstValue("appid");
            if (string.IsNullOrEmpty(requestUserId))
            {
                return Unauthorized(new { message = "User ID from the token is missing or invalid." });
            }
            Category check = await _categoryService.GetCategoryById(source.CategoryId);


            if (check == null)
            {
                return BadRequest(new CustomErrorResponse
                {
                    Type = "Category not found",
                    Title = "One or more errors occurred.",
                    Status = 400,
                    Message = "Category not found",
                    TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }

            UserDto userDetails = null;

            var rpcClient = new RpcClient("users");
            Console.WriteLine(" [x] Requesting user details");
            var response = await rpcClient.Call(requestUserId.ToString());
            userDetails = JsonSerializer.Deserialize<UserDto>(response);

            if (userDetails.Email == null)
            {
                return BadRequest(new CustomErrorResponse
                {
                    Type = "User not found",
                    Title = "One or more errors occurred.",
                    Status = 400,
                    Message = "User not found",
                    TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(source.File.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest(new CustomErrorResponse
                {
                    Type = "Invalid file type",
                    Title = "One or more errors occurred.",
                    Status = 400,
                    Message = "Only PDF, Word documents, or image files are allowed",
                    TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }

            var fileName = await _fileService.Upload(source.File, requestUserId, source.privacy);


            var sourceModel = _mapper.Map<Source>(source);
            sourceModel.FileName = fileName.FileName;
            sourceModel.UrlFile = fileName.UrlFile;
            sourceModel.UserId = requestUserId;


            var newSource = await _sourceService.AddSource(sourceModel);


            var expiryTime = DateTimeOffset.Now.AddSeconds(120);

            var sourceDto = _mapper.Map<SourceDto>(newSource);
            sourceDto.Category = _mapper.Map<CategoryDto>(check);
            sourceDto.User = userDetails;
            if (source.privacy == "private")
            {
                var saveSAS = await _fileStorageService.AddFileStorage(new FileStorage
                {
                    SourceId = newSource.Id,
                    Token = fileName.Token,
                });

                sourceDto.UrlFile = saveSAS.Token;
            }
            else
            {
                sourceDto.UrlFile = fileName.UrlFile;
            }


            _cacheService.SetData<SourceDto>($"source{newSource.Id}", sourceDto, expiryTime);
            _cacheService.RemoveData($"sourcesuser{requestUserId}");
            _cacheService.RemoveBySubstring("False");
            _cacheService.RemoveBySubstring("True");
            Console.WriteLine("all keys " + _cacheService.GetAllKeys());


            return Ok(sourceDto);
        }




        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSource(string id, [FromBody] CreateSourceDto source)
        {
            var requestUserId = User.FindFirstValue("appid");
            if (string.IsNullOrEmpty(requestUserId))
            {
                return Unauthorized(new { message = "User ID from the token is missing or invalid." });
            }
            var sourceModel = await _sourceService.GetSource(id);
            if (sourceModel == null)
            {
                return NotFound();
            }
            sourceModel.Title = source.Title;
            sourceModel.Description = source.Description;
            sourceModel.CategoryId = source.CategoryId;
            sourceModel.UserId = requestUserId;
            var updatedSource = await _sourceService.UpdateSource(sourceModel);
            return Ok(updatedSource);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSource(string id)
        {
            var source = await _sourceService.GetSource(id);
            if (source == null)
            {
                return NotFound();
            }
            var deletedSource = await _sourceService.DeleteSource(id);
            if (deletedSource.privacy == "private")
            {
                await _fileStorageService.DeleteFileStorage(deletedSource.Id);
            }
            _cacheService.RemoveData($"source{deletedSource.Id}");
            return Ok(deletedSource);
        }

        private async Task<bool> IsUserServiceAvailable()
        {
            try
            {
                var rpcClient = new RpcClient("users");

                var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5));
                var responseTask = rpcClient.Call("ping");

                var completedTask = await Task.WhenAny(responseTask, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    return false;
                }
                else
                {
                    var response = await responseTask;
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpDelete("user")]
        [Authorize]
        public async Task<ActionResult> DeleteSourcesByUserId()
        {
            var requestUserId = User.FindFirstValue("appid");
            Console.WriteLine("Request user ID: " + requestUserId);
            if (string.IsNullOrEmpty(requestUserId))
            {
                return Unauthorized(new { message = "User ID from the token is missing or invalid." });
            }
            try
            {
                var sources = await _sourceService.DeleteSourcesByUserId(requestUserId);
                foreach (var source in sources)
                {
                    if (source.privacy == "private")
                    {
                        await _fileStorageService.DeleteFileStorage(source.Id);
                    }
                    _cacheService.RemoveData($"source{source.Id}");
                }
                _cacheService.RemoveData($"sourcesuser{requestUserId}");
                _cacheService.RemoveBySubstring("False");
                _cacheService.RemoveBySubstring("True");
                Console.WriteLine("all keys " + _cacheService.GetAllKeys());
                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return BadRequest("User ID cannot be null or empty.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }





    }
}