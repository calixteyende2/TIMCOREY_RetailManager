﻿using System;

namespace TRMWPFUserInterface.Library
{
    public interface ILogInUserModel
    {
        DateTime CreatedDate { get; set; }
        string EmailAddress { get; set; }
        string FirstName { get; set; }
        string Id { get; set; }
        string LastName { get; set; }
        string Token { get; set; }

        void ResetUser();
    }
}