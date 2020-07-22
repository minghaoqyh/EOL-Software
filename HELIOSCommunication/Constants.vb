Module ConstantsCan
    Public Const CAPPLICATION As String = "160C2201"
    Public Const CLIGHTCTRL As String = "160C2282"
    Public Const CFILTERWHEEL As String = "160C2306"
    Public Const CNAMEPLATE As String = "160C2202"
    Public Const CDEBUG As String = "160C220D"
    Public Const CTEMP As String = "160c2314"
    Public Const CBULBMANAGMENT As String = "160C2303"
    Public Const CERROR As String = "160C2207"
    Public Const CBOOTLOADER As String = "160C26AE"

    Public Const CAN_SOURCE As Integer = 1
    Public Const CAN_DES As Integer = 3
    Public Const CAN_DES_ALT As Integer = 13

    Public Enum eFeatureP
        GETMIN = 0
        GETCURRENT = 1
        GETSTATIC = 2
        GETMAX = 3
        GETFACTOR = 5
        GETFEATURE = 6
        GETINIT = 7
        SETMIN = 8
        SETCURRENT = 9
        SETSTATIC = 10
        SETMAX = 11
        SETFACTOR = 13
        SETINIT = 15
    End Enum

    Public Enum eFeatureM
        EXECUTE0 = 0
    End Enum

    Public Enum eProtocol
        GENERICBASEMODUL = 0
        GENERICFCTMODULE = 1
        NON_MODULE_CMD = 2
        SPECIFICAL_FCT_MODULE = 3
    End Enum

    Public Enum eCBootloaderP
        BOOTLOADER = 8
    End Enum

    Public Enum eCNamePlateP
        APPLICATIONTYPE = 0
        APPLICATIONVERSION = 1
        TYPEANDREVISION = 2
        HWSUBSYSTEMTYPE = 3
        BOARDTYPE = 4
        DEVICESNANDREV = 5
    End Enum

    Public Enum eCLightCtrlP
        LIGHTVALUE = 0
        ONOFFSTATE = 1
        BULBLIFETIME = 2
        BULBVOLTAGE = 3
        BULBCURRENT = 4
        VELOCITY = 5
        OUTPUTPOWER = 6
        LIGHTLIMIT = 7
        LIGHTFACTOR = 8
        TYPO = 9
        COLORTEMP = 10
        FLASHTIMER = 11
    End Enum

    Public Enum eCLightCtrlM
        FLASH = 34
    End Enum

    Public Enum eCFilterWheelP
        FILTERPOS = 0

    End Enum

    Public Enum eCTempP
        TEMPERATURE = 0
    End Enum

    Public Enum eCBulbManagmentP
        BURNTIME = 1
    End Enum

    Public Enum eCErrorP
        LASTERROR = 0
        ERRORQUEUE = 1
        EXTENDEDDATA = 2
    End Enum

    Public Enum eCDebugP
        FAILSAFETEST = 0
        CALIBRATIONDATARGB = 1
        SERIALNUMBERSRGB = 2
        BULBVOLTAGERGB = 3
        BULBCURRENTRGB = 4
        PHOTODIODERGB = 5
        PHIRGB = 6
        RAWPOSITION = 8
        ERRORINFO = 10
        VERSIONNUMBERCMS = 11
        INTENSITYANDCOLORTEMP = 12
        SYSTEMSETTINGS = 13
    End Enum

    Public Enum eCDebugM
        OPENRGB = 32
        WIPE = 34
        CLOSERGB = 35
    End Enum

    Public Enum eCNamePlateM
        OPEN = 32
        CLEAR = 33
        FORMAT = 34
        CLOSE = 35
    End Enum

    Public Enum eCApplicationM
        SUPERR9MODE = 30
        ASYNRESET = 32
    End Enum

End Module
