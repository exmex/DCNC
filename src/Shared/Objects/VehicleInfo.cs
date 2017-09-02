using System.Collections.Generic;
using System.IO;

namespace Shared.Objects
{
    public class VehicleInfo
    {
        public enum XIVEHICLEGRADE_TYPE
        {
            eXI_GRADETYPE_V = 0x0,
            eXI_GRADETYPE_R = 0x1,
            eXI_GRADETYPE_Q = 0x2,
            eXI_GRADETYPE_MAX = 0x3,
        };

        public enum XIVEHICLETYPE
        {
            eXI_TYPE_PLAYERCAR = 0x0,
            eXI_TYPE_HUV = 0x1,
            eXI_TYPE_TRAFFIC = 0x2,
            eXI_TYPE_RACINGBATTLE = 0x3,
            eXI_VEHICLETYPE_MAX = 0x4,
        };

        public int bModel;
        public uint dwID;
        public int bSellable;
        public int bCloseStage;
        public int nAutoShopDisplayOrder;
        public int bAeroSetType;
        public XIVEHICLETYPE eVehicleType;
        public string szVehicleTypeDesc;
        public string szMaker;
        public string szName;
        public string szShortName;
        public string szFileName;
        public string szOldFileName;
        public int nAccel;
        public int nSpeed;
        public int nCrash;
        public int nBoost;
        public int nReqLevel;
        public XIVEHICLEGRADE_TYPE eVehicleGradeType;
        public int nVehicleGradeLvl;
        public float fLength;
        public float fWidth;
        public float fHeight;
        public float fFrontLength;
        public float fRearLength;
        public float fRatio;
        public float fWeight;
        public char[,] Color; // 4,2
        public float fWheelScale;
        public int iWheelCount;
        public int iRearWheelCount;
        public int[] WheelID; // 10
        public string tszTireGroupID;
        public int nSpoiler;
        public int nNumberPlate;
        public int nSlotSpeed;
        public int nSlotAccel;
        public int nSlotCrash;
        public int nSlotBoost;
        public float fTurboWeakenFactor;
        public float fNoSlipTime;
        public float fJumpScale;
        public float fMaxHeightDiff;
        public float fCamBackAdd;
        public float fCamHeightAdd;
        public float fAccelation;
        public float fDeAccelation;

        //VEHICLE_INFO::PLANE_SHADOW shadow;
        public int shadow_car0;

        public int shadow_car1;
        public int m_BodyIdx;
        public uint m_InstanceID;
        public int bIsJumpCar;

        public static void Load(string file)
        {
            var levelTable = new Dictionary<ulong, VehicleInfo>();
            using (TextReader reader = File.OpenText(file))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var data = line.Split(',');
                    //Index
                    //Support
                    //Vehicle
                    //Desc
                    //Maker
                    //Car Name
                    //UI Name
                    //File Name
                    //OldFileName
                    //Unique Id
                    //Sellable
                    //closestage
                    //Display
                    //GradeType
                    //AeroSet
                    //Accel
                    //Speed
                    //Crash
                    //Boost
                    //Req Level
                    //Grade
                    //Lvl
                    //length
                    //width
                    //height
                    //fRatio
                    //flength
                    //rlength
                    //shadow1
                    //shadow2
                    //Body0
                    //Window
                    //Body0
                    //Window
                    //Body0
                    //Window
                    //Body0
                    //Window
                    //Tire Scale (Diameter(mm))
                    //TireCnt
                    //RearCnt
                    //Tire Id_01
                    //Tr02
                    //Tr03
                    //Tr04
                    //Tr05
                    //Tr06
                    //Tr07
                    //Tr08
                    //Tr09
                    //Tr10
                    //Tire Group Id
                    //Spoiler
                    //Number Plate
                    //Speed slot
                    //Accel slot
                    //Crash slot
                    //Boost slot
                    //weight
                    //turboWeakenFactor
                    //noSlipTime
                    //jumpScale
                    //maxHeightDiff
                    //CamN
                    //CamH
                    //Accelation
                    //DeAccelation
                    //JumpCar?
                    //MagicNumber
                }
            }
        }
    }
}