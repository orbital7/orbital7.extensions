namespace System;

public static class MimeTypesHelper
{
    public const string FILE_EXT_PNG = ".png";
    public const string MIME_TYPE_PNG = "image/png";

    public const string FILE_EXT_JPG = ".jpg";
    public const string FILE_EXT_JPEG = ".jpeg";
    public const string MIME_TYPE_JPEG = "image/jpeg";

    public const string FILE_EXT_GIF = ".gif";
    public const string MIME_TYPE_GIF = "image/gif";

    public const string FILE_EXT_SVG = ".svg";
    public const string MIME_TYPE_SVG = "image/svg+xml";

    public const string FILE_EXT_TXT = ".txt";
    public const string MIME_TYPE_TXT = "text/plain";

    public const string FILE_EXT_HTML = ".html";
    public const string MIME_TYPE_HTML = "text/html";

    public const string FILE_EXT_JSON = ".json";
    public const string MIME_TYPE_JSON = "application/json";

    public const string FILE_EXT_PDF = ".pdf";
    public const string MIME_TYPE_PDF = "application/pdf";

    public const string FILE_EXT_DOCX = ".docx";
    public const string MIME_TYPE_DOCX = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

    public const string FILE_EXT_XLSX = ".xlsx";
    public const string MIME_TYPE_XLSX = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    public const string FILE_EXT_CSV = ".csv";
    public const string MIME_TYPE_CSV = "text/csv";

    public const string FILE_EXT_BMP = ".bmp";
    public const string MIME_TYPE_BMP = "image/bmp";

    public const string FILE_EXT_TIF = ".tif";
    public const string FILE_EXT_TIFF = ".tiff";
    public const string MIME_TYPE_TIFF = "image/tiff";

    public const string FILE_EXT_EMF = ".emf";
    public const string FILE_EXT_WMF = ".wmf";
    public const string MIME_TYPE_WMF = "application/x-msmetafile";

    public static string FileExtensionToMimeType(
        string fileExtension)
    {
        switch (fileExtension.ToLower())
        {
            case FILE_EXT_PNG:
                return MIME_TYPE_PNG;

            case FILE_EXT_JPG:
            case FILE_EXT_JPEG:
                return MIME_TYPE_JPEG;

            case FILE_EXT_GIF:
                return MIME_TYPE_GIF;

            case FILE_EXT_SVG:
                return MIME_TYPE_SVG;

            case FILE_EXT_TXT:
                return MIME_TYPE_TXT;

            case FILE_EXT_HTML:
                return MIME_TYPE_HTML;

            case FILE_EXT_JSON:
                return MIME_TYPE_JSON;

            case FILE_EXT_PDF:
                return MIME_TYPE_PDF;

            case FILE_EXT_DOCX:
                return MIME_TYPE_DOCX;

            case FILE_EXT_XLSX:
                return MIME_TYPE_XLSX;

            case FILE_EXT_CSV:
                return MIME_TYPE_CSV;

            case FILE_EXT_BMP:
                return MIME_TYPE_BMP;

            case FILE_EXT_TIF:
            case FILE_EXT_TIFF:
                return MIME_TYPE_TIFF;

            case FILE_EXT_EMF:
            case FILE_EXT_WMF:
                return MIME_TYPE_WMF;

            default:
                return null;
        }
    }

    public static string MimeTypeToFileExtension(
        string mimeType)
    {
        switch (mimeType.ToLower())
        {
            case MIME_TYPE_PNG:
                return FILE_EXT_PNG;

            case MIME_TYPE_JPEG:
                return FILE_EXT_JPG;

            case MIME_TYPE_GIF:
                return FILE_EXT_GIF;

            case MIME_TYPE_SVG:
                return FILE_EXT_SVG;

            case MIME_TYPE_TXT:
                return FILE_EXT_TXT;

            case MIME_TYPE_HTML:
                return FILE_EXT_HTML;

            case MIME_TYPE_JSON:
                return FILE_EXT_JSON;

            case MIME_TYPE_PDF:
                return FILE_EXT_PDF;

            case MIME_TYPE_DOCX:
                return FILE_EXT_DOCX;

            case MIME_TYPE_XLSX:
                return FILE_EXT_XLSX;

            case MIME_TYPE_CSV:
                return FILE_EXT_CSV;

            case MIME_TYPE_BMP:
                return FILE_EXT_BMP;

            case MIME_TYPE_TIFF:
                return FILE_EXT_TIF;

            case MIME_TYPE_WMF:
                return FILE_EXT_WMF;

            default:
                return null;
        }
    }

    public static bool IsBitmapMimeType(
        string mimeType)
    {
        switch (mimeType.ToLower())
        {
            case MIME_TYPE_PNG:
            case MIME_TYPE_JPEG:
            case MIME_TYPE_GIF:
            case MIME_TYPE_BMP:
            case MIME_TYPE_TIFF:
                return true;
            default:
                return false;
        }
    }

    public static bool IsBitmapFileExtension(
        string fileExtension)
    {
        switch (fileExtension.ToLower())
        {
            case FILE_EXT_PNG:
            case FILE_EXT_JPG:
            case FILE_EXT_JPEG:
            case FILE_EXT_GIF:
            case FILE_EXT_BMP:
            case FILE_EXT_TIF:
            case FILE_EXT_TIFF:
                return true;
            default:
                return false;
        }
    }

    public static List<string> GetBitmapFileExtensions()
    {
        return new List<string>
        {
            FILE_EXT_PNG,
            FILE_EXT_JPG,
            FILE_EXT_JPEG,
            FILE_EXT_GIF,
            FILE_EXT_BMP,
            FILE_EXT_TIF,
            FILE_EXT_TIFF,
        };
    }

    public static bool IsMetaFileMimeType(
        string mimeType)
    {
        switch (mimeType.ToLower())
        {
            case MIME_TYPE_WMF:
            case MIME_TYPE_SVG:
                return true;
            default:
                return false;
        }
    }

    public static bool IsMetaFileFileExtension(
        string fileExtension)
    {
        switch (fileExtension.ToLower())
        {
            case FILE_EXT_EMF:
            case FILE_EXT_WMF:
            case FILE_EXT_SVG:
                return true;
            default:
                return false;
        }
    }

    public static List<string> GetMetaFileFileExtensions()
    {
        return new List<string>
        {
            FILE_EXT_EMF,
            FILE_EXT_WMF,
            FILE_EXT_SVG,
        };
    }
}
