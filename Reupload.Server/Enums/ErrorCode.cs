namespace Reupload.Server.Enums;

public enum ErrorCode
{
    General = 0,
    Mapping = 1,
    Validation = 2,

    UserNotFound = 10,

    PackageNotFound = 20,
    PackageReachedPhotoUploadLimit = 21,

    PostNotFound = 30,

    UserActionNotFound = 40
}