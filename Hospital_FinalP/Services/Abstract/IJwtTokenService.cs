﻿namespace Hospital_FinalP.Services.Abstract
{
    public interface IJwtTokenService
    {
        public string GenerateToken( string fullName, string userName, List<string> roles, int? patientId = null, int? doctorId = null);
    }

}
