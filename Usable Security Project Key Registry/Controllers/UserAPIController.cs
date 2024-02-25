using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using Usable_Security_Project_Key_Registry.Models;
using Usable_Security_Project_Key_Registry.Models.DTO;
using Usable_Security_Project_Key_Registry.Services.User_Services;

namespace Usable_Security_Project_Key_Registry.Controllers
{
    [Route("api/Users")]
    [ApiController]

    public class UserAPIController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserAPIController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok(_userService.Get());
        }

        [HttpGet("GetById/{Id:int}", Name = "GetUserById")]
        public IActionResult GetById(int Id)
        {
            if (Id == 0)
            {
                ModelState.AddModelError("Invalid ID", "You entered an invalid ID!");
                return BadRequest(ModelState);
            }


            User? user = _userService.GetByID(Id);

            if (user == null)
            {
                ModelState.AddModelError("User Not Found", "No user associated with this ID was found!");
                return NotFound(ModelState);
            }

            return Ok();
        }

        [HttpGet("GetByEmail/{email}", Name = "GetUserByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByEmail(string email)
        {
            if (email == null || email == "")
            {
                ModelState.AddModelError("Null Email", "You did not enter an email!");
                return BadRequest(ModelState);
            }

            User? user = _userService.GetByEmail(email);

            if (user == null)
            {
                ModelState.AddModelError("User Not Found", "No user associated with this email was found!");
                return NotFound(ModelState);
            }


            return Ok(user);
        }

        [HttpGet("GetByPublicKey/{publicKey}", Name = "GetUserByPublicKey")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByPublicKey(string publicKey)
        {
            if (publicKey == null || publicKey == "")
            {
                ModelState.AddModelError("Null Public Key", "You did not enter a public key!");
                return BadRequest(ModelState);
            }

            if (publicKey == "No Public Key!")
            {
                ModelState.AddModelError("Public Key Not Found", "No public associated with this email was found!");
                return NotFound(ModelState);
            }

            User? user = _userService.GetByPublicKey(publicKey);

            if (user == null)
            {
                ModelState.AddModelError("User Not Found", "No user associated with this public key was found!");
                return NotFound(ModelState);
            }


            return Ok(user);
        }

        [HttpGet("GetPublicKey/{email}", Name = "GetPublicKey")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPublicKey(string email)
        {
            if (email == null || email == "")
            {
                ModelState.AddModelError("Null Email", "You did not enter an email!");
                return BadRequest(ModelState);
            }
            if (!IsEmailValid(email))
            {
                ModelState.AddModelError("Invalid Email", "The email that was entered is invalid!");
                return BadRequest(ModelState);
            }
            string publicKey = _userService.GetPublicKey(email);

            if (publicKey == "No Public Key!")
            {
                ModelState.AddModelError("Public Key Not Found", "No public associated with this email was found!");
                return NotFound(ModelState);
            }

            return Ok(publicKey);
        }



        [HttpPost("AddUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddUser([FromBody] UserDTO userDTO)
        {
            if (userDTO.Email == null)
            {
                ModelState.AddModelError("Null Email", "You did not enter an email!");
                return BadRequest(ModelState);
            }
            if (!IsEmailValid(userDTO.Email))
            {
                ModelState.AddModelError("Invalid Email", "The User Email that was entered is invalid!");
                return BadRequest(ModelState);
            }

            string privateKey = _userService.Add(userDTO);

            return StatusCode(StatusCodes.Status201Created, privateKey);

        }

        [HttpPost("CheckEmail", Name = "CheckEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult CheckEmail(string email)
        {
            if (email == null || email == "")
            {
                ModelState.AddModelError("Null Email", "You did not enter an email!");
                return BadRequest(ModelState);
            }
            if (!IsEmailValid(email))
            {
                ModelState.AddModelError("Invalid Email", "The email that was entered is invalid!");
                return BadRequest(ModelState);
            }

            if (_userService.CheckEmail(email))
                return Ok("YES");

            ModelState.AddModelError("User Not Found", "No user associated with this email was found!");
            return NotFound(ModelState);

        }

        [HttpPost("CheckQR")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CheckQR([FromBody] UserDTO userDTO)
        {
            if (userDTO.Email == null)
            {
                ModelState.AddModelError("Null Email", "You did not enter an email!");
                return BadRequest(ModelState);
            }
            if (!IsEmailValid(userDTO.Email))
            {
                ModelState.AddModelError("Invalid Email", "The User Email that was entered is invalid!");
                return BadRequest(ModelState);
            }

            if(userDTO.QRSignature == null)
            {
                ModelState.AddModelError("Null QR Signature", "You did not enter a QR signature!");
                return BadRequest(ModelState);
            }


            if (_userService.CheckQR(userDTO.Email, userDTO.QRSignature))
                return Ok("YES");


            ModelState.AddModelError("Invalid QR Signature", "The QR signature is not associated with this email!");
            return NotFound(ModelState);


        }

        [HttpPost("CheckPIN")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CheckPIN([FromBody] UserDTO userDTO)
        {
            if (userDTO.Email == null)
            {
                ModelState.AddModelError("Null Email", "You did not enter an email!");
                return BadRequest(ModelState);
            }
            if (!IsEmailValid(userDTO.Email))
            {
                ModelState.AddModelError("Invalid Email", "The User Email that was entered is invalid!");
                return BadRequest(ModelState);
            }

            if (userDTO.PINSignature == null)
            {
                ModelState.AddModelError("Null PIN Signature", "You did not enter a PIN signature!");
                return BadRequest(ModelState);
            }


            if (_userService.CheckPIN(userDTO.Email, userDTO.PINSignature))
                return Ok("YES");


            ModelState.AddModelError("Invalid PIN Signature", "The PIN signature is not associated with this email!");
            return NotFound(ModelState);


        }

        

        private static bool IsEmailValid(string email)
        {
            var valid = true;

            if (email.Any(Char.IsWhiteSpace))
            {
                return false;
            }
            else
            {
                try
                {
                    var emailAddress = new MailAddress(email);
                }
                catch
                {
                    valid = false;
                }

                return valid;
            }


        }



    }
}
