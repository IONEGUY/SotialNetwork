using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SotialNetwork.Api.Models
{
    public enum ErrorCode
    {
        // General
        ValidationError = 1001,

        // Registration
        DuplicateUser = 1002,
        EmailInUseByAdmin = 1003,
        ResetPasswordTokenCorrupted = 1004,
        CannotChangeSelfRole = 1005,

        // Forgot password
        EmailNotFound = 1010,

        // Reports
        TrolleyReportNotFound = 1101,

        // Users
        CannotChangeCurrentUserRole = 1201,
        PropertyCannotBeUpdated = 1202
    }
}