namespace Domain.Common;

public class Common
{
    // Added by Alex Ng Nkh, 19/01/2022, 31 Aug 2022, MyDebit CNP (NKH002CNP)
    public enum CaptureType
    {
        DefaultCap = 0,
        OnlyFullCap = 1
    }

    // Added by Alex Ng Nkh, 27/10/2021, 28 Aug 2022, MyDebit CNP (NKH001CNP)
    public enum DBCardIssuing
    {
        GetIssuerInfo = 0,
        GetIssuerFiid = 1
    }

    public enum ERROR_CODE
    {
        E_SUCCESS = 0,
        E_TXN_SUCCESSFUL = 1, //From NA--
        E_TXN_INITIATED = 2, //From NA--
        E_TXN_AUTHORIZED = 3, //From NA--
        E_TXN_FAILED = 4, //From NA--
        E_COM_TIMEOUT = 5, //From NA--
        E_TXN_REVERSED = 7, //From NA--
        E_TXN_VOIDED = 8, //From NA
        E_TXN_HOTCARD = 9,

        E_TXN_PENDING_SALE_RESP = 30, //From NA
        E_TXN_PENDING_REVERSAL_RESP = 31, //From NA
        E_TXN_PENDING_VOID_RESP = 32, //From NA
        E_TXN_PENDING_AUTH_RESP = 33, //From NA

        E_INVALID_PAN = 101, //From NA
        E_INVALID_PAN_LENGTH = 102, //From NA
        E_INVALID_PAN_UNKNOWN = 103, //From NA
        E_INVALID_TRACK2_FORMAT = 104, //From NA

        E_DB_UNKNOWN_ERROR = 901, // From NA--
        E_INVALID_HOST = 902, // From NA--
        E_FAILED_GET_REV_RESP = 903, //From NA--
        E_FAILED_REVERSAL = 904, // From NA--
        E_FAILED_SET_TXN_STATUS = 905, //From NA--
        E_FAILED_STORE_TXN = 906, // Failed bInsertNewTxn
        E_FAILED_UPDATE_TXN_RESP = 907, // From NA--
        E_FAILED_FORM_REQ = 914, // From NA--
        E_FAILED_COM_INIT = 917, // From NA--

        E_COMMON_INVALID_FIELD = 2902,
        E_COMMON_MISSING_ELEMENT = 2903,

        E_INVALID_INPUT = 2906, // Failed bCheckUniqueTxn
        E_INVALID_HOST_REPLY = 2907,
        E_INVALID_MID = 2901,
        E_INVALID_SIGNATURE = 2909,

        E_COMMON_UNKNOWN_ERROR = 2999, // Failed GenTxnID, bInsertTxnResp, bCheckUniqueTxn, bInsertNewTxn


        //E_PAN_AUTH_NOT_AVAILABLE = 16,
        //E_PAN_NOT_PARTICIPATING = 17,

        //E_DB_INVALID_FIELD = 101,
        //E_DB_INVALID_TYPE = 102,
        //E_DB_MISSING_FIELD = 106,
        //E_DB_FAIL_INSERT = 111,
        //E_DB_FAIL_UPDATE = 112,
        //E_DB_FAIL_DELETE = 113,
        //E_DB_FAIL_QUERY = 114,

        //E_CERT_INVALID_FILE = 141,
        //E_CERT_FILE_EXISTED = 142,
        //E_CERT_SIGNATURE_FAILED = 143,
        //E_CERT_CHAIN_INCOMPLETE = 144,
        E_CERT_ERROR = 51,

        E_COM_BAD_REQUEST = 41,
        E_COM_UNAUTHORIZED_URL = 42,
        E_COM_FORBIDDEN_URL = 43,
        E_COM_URL_UNREACHABLE = 44,
        E_COM_FILE_ERROR = 45,

        //E_MERCHANT_DOES_NOT_EXIST = 241,
        //E_MERCHANT_ALREADY_EXIST = 242,
        //E_MERCHANT_INACTIVE = 243,
        //E_MERCHANT_PWD_MISMATCH = 244,
        //E_MERCHANT_PWD_FORMAT = 245,
        //E_MERCHANT_ACQ_INVALID = 246,
        //E_MERCHANT_INVALID_CURRENCY = 247,

        E_CRYPTO_INIT_KEY = 51,
        E_CRYPTO_FAIL = 52,
        E_CRYPTO_UNKNOWN_ERROR = 53,

        //E_CRANGE_NOT_IN_RANGE = 331,
        //E_CRANGE_INVALID_BIN = 332

        E_TXN_FAIL_GEN_ID = 61,
        E_TXN_INVALID_STATE = 62,
        E_TXN_NOT_FOUND = 63,
        E_TXN_INVALID_AMOUNT = 64,

        E_CAPTURE_FAILED = 500,

        E_VC_INVALID_DATA = 400,
        E_VC_INVALID_DATA_ACCESS_LEVEL = 401,
        E_VC_INVALID_KEY_ID_ACCOUNT = 403,
        E_VC_INVALID_CALLID = 404,
        E_VC_INVALID_TOKEN = 409
    }

    // Added by LL, 23 Jun 2021, VIS flow correction
    public enum INSTALLMENT_FREQ
    {
        WEEKLY = 1,
        BIWEEKLY = 2,
        MONTHLY = 3
    }

    //0 - MPI transaction Started
    //1 - MPI Formulates PaReq and sends back to Gateway
    //2 - MPI Successful Callback Received
    //3 - MPI Failed Callback Received
    //4 - MPI Could not formulate PAReq due to some error
    //5 - Not Support MPI2.0. BAU
    //6 - Pending Callback Wait For Requery
    public enum MPI_TXN_STATUS
    {
        MPI_Transaction_Started = 0,
        MPI_Formulates_PaReq_and_sends_to_Gateway = 1,
        MPI_Success_CallBack = 2,
        MPI_Failed_CallBack = 3,
        MPI_Error_Formating_PAReq = 4,
        MPI_Not_Support_MPI2_BAU = 5,
        MPI_Pending_CallBack_Requery = 6
    }

    // Added by Alex Ng Nkh, 19/01/2022, 31 Aug 2022, MyDebit CNP (NKH002CNP)
    public enum QueryFlag
    {
        QueryOwnDb = 2,
        NormalQuery = 1,
        NoQuery = 0,
        NoQueryDoCapRetry = -1
    }

    // Added by Alex Ng Nkh, 19/01/2022, 31 Aug 2022, MyDebit CNP (NKH002CNP)
    public enum RSalePermit
    {
        NotAllowAll = 0,
        AllowAll = 1,
        IgnoreAmtCheck = 2,
        AutoVoidSuccededTxnError = 3,
        AllowAllExceptCap = 4
    }

    public enum SERVICE_TYPE // Added by Jeff, 23 Jul 2018
    {
        E_BANK_DIRECT = 1,
        E_G_DIRECT = 2
    }

    // Added By Jawad, 30 May 2017, SOP
    public enum SOP_STATUS
    {
        TOKEN_CREATED = 1,
        TOKEN_SENT = 2,
        TOKEN_USED = 3
    }

    public enum TXN_STATE
    {
        INITIAL = 1, //Received request for a particular txn for the 1st time from AA Web or other merchants

        SENT_TO_HOST =
            2, //Can be redirect through client's browser to Host,e.g.CIMB(popup) OR sent through WebComm to Host  'PENDING_PROCESS = 2
        DECLINE_FDS = 3, //Added by Jeff, 12 Oct 2013, declined by FDS

        SUCCESS = 4,
        FAILED = 5, // Failed spInsTxnResp, bCheckUniqueTxn, bInsertNewTxn
        ABORT = 6, // Failed GenTxnID, GetData, bPaymentMain
        ROLLBACK_PENDING = 7, //Reversal timeout/pending
        ROLLBACK_SUCCESS = 8, //Reversal success
        COMMIT_PENDING = 9,
        COMMIT = 10,
        ROLLBACK_FAILED = 11, //Reversal failed
        COMMIT_FAILED = 12,

        RECV_ADD_SEATS =
            13, //Received add seats reply from OpenSkies/NewSkies   - Added on 16 Aug 2009, to cater for add seats

        SENT_2ND_ENTRY = 15, //Replied an entry page to client, e.g. UserID entry(BCA); Tokens info entry(Mandiri)
        RECV_2ND_ENTRY = 16, //e.g. Received UserID (BCA);Tokens info(Mandiri)                  '    'PROCESSING = 3
        RECV_FROM_HOST = 17, //Received reply from Host
        SENT_QUERY_HOST = 18, //Sent query request to Host
        RECV_QUERY_HOST = 19, //Received query reply from Host
        SENT_QUERY_OS = 20, //Sent query request to OpenSkies/NewSkies
        RECV_QUERY_OS = 21, //Received query reply from OpenSkies/NewSkies
        SENT_CONFIRM_OS = 22, //Sent Add/Confirm Payment request to OpenSkies/NewSkies
        RECV_CONFIRM_OS = 23, //Received Add/Confirm Payment reply from OpenSkies/NewSkies
        RECV_HOST_QUERY = 24, //Received Host's Query request, e.g. BCA
        SENT_HOST_QUERY = 25, //Sent Host's Query reply, e.g. BCA
        SENT_REPLY_CLIENT = 26, //Sent reply to client/merchant
        SENT_RSALE_HOST = 27, //Sent Reversal Sale request to Host     - Added on 10 Apr 2010
        RECV_RSALE_HOST = 28, //Received Reversal Sale reply from Host - Added on 10 Apr 2010
        SENT_REFUND_HOST = 29, //Sent Refund request to Host            - Added by OoiMei, 8 July 2015
        RECV_REFUND_HOST = 30, //Received Refund reply from Host        - Added by OoiMei, 8 July 2015

        INSTL_NOT_ENTITLE =
            31, //Installment Failed due to not able to get routing based on card BIN range. Added by OoiMei, 25 May 2016
        FAILED_REVERSAL = 65 //Added by XinYi, 31 Aug 2022, MyDebit CNP
    }

    public enum TXN_STATUS
    {
        TXN_SUCCESS = 0, //1
        TXN_FAILED = 1, //4
        TXN_PENDING = 2, //Processing

        TXN_PENDING_CONFIRM_BOOKING =
            3, //Host returned approved for the payment, pending OpenSkies AddPymt reply OR trying to send AddPymt request to OpenSkies, need extend booking

        TXN_TIMEOUT_HOST_RETURN =
            4, //Host no response for 1 hour 45 minutes, need extend booking, stop and generate exception report

        TXN_TIMEOUT_HOST_UNPROCESSED =
            5, //Host's reply status is "not processed" for 1 hour 45 minutes, no need extend booking, no need exception report
        TIME_OUT_FAILED = 6, //Txn existed in Request table for more than 2 hours
        TXN_HOST_UNPROCESSED = 7, //Host's reply status is "not processed"
        TXN_HOST_RETURN_TIMEOUT = 8, //Host no response/timeout
        TXN_REVERSED = 9, //Txn reversed successfully
        TXN_NOT_FOUND = -1, //Added by Jeff, 3 Sept 2013
        TXN_INT_ERR = -2, //Added by Jeff, 3 Sept 2013
        TXN_EXPIRED_IN_PYMTPAGE = -3, //Added by OoiMei, 1 Nov 2017. Expired in Payment Page

        TXN_AUTH_SUCCESS =
            15, //Added by OoiMei, 14 April 2015. Auth&Capture. Txn status Auth Success returned to merchant in Query resp

        TXN_CAPTURE_SUCCESS =
            16, //Added by OoiMei, 14 April 2015. Auth&Capture. Txn status Capture Success returned to merchant in Query resp

        TXN_REFUND_SUCCESS =
            10, //Added by Jeff, 31 May 2015. Refund. Txn status Refund Success returned to merchant in Query resp
        TXN_CANCELLED = 17, //Added by HeanSeng, 27 Mar 2018

        TXN_MPI_AUTHENTICATION_SUCCESS =
            28, //Added by Christian C., 7 Jan 2019. Modified by Jeff, 15 Feb 2019, 18 to 28, since 18 indicates Partial Refund

        PENDING_RESP = 30,
        PENDING_REVERSAL = 31,
        PROCESSING_RESP = 32,

        // QUERYING_HOST = 33                 // Commented by Jeff, 1 Dec 2015
        PENDING_REFUND = 33, // Added by OoiMei, 8 Jul 2015
        PENDING_CAPTURE = 34, // Added by OoiMei, 5 Aug 2016

        EXCEED_PER_TXN_AMT_LIMIT = 50, // Added by Jeff, 16 Jan 2014
        DECLINED_BY_EXT_FDS = 51, // Added by Jeff, 12 Jul 2016, FraudWall
        DECLINED_BY_PROMOTION = 52, // Added by HFM, 07 Oct 2016, Promotion
        DECLINED_BY_INT_FDS = 53, // Added by LeeLin, 28 Feb 2019, Malindo

        FAILED_STORE_TXN = 906, // Failed bInsertNewTxn
        E_INVALID_MERCHANT = 2901,
        INVALID_INPUT = 2906, // Failed GetData, bInitPayment, bPaymentMain
        INVALID_HOST_REPLY = 2907,
        LATE_HOST_REPLY = 2908, // Late reply from Host
        INVALID_HOST_IP = 2909,
        INVALID_HOST_REPLY_GATEWAYTXNID = 2910,
        INVALID_HOST = 2911,
        INTERNAL_ERROR = 2999, // Failed bInitPayment, bProcessSale
        FAILED_REVERSAL = 64 // Added by XinYi, 31 Aug 2022, MyDebit CNP
    }


    public enum TXN_STATUS_2
    {
        ExpiredInPaymentPage = -3,
        PendingAtPaymentPage = -2,
        Init = -1,
        Success = 0,
        Failed = 1,
        Pending = 2,
        ProductFulfillmentPendingOrTimeout = 3,
        QueryHostTimeout = 4,
        NotProcessedbyBank = 5,
        Expired = 6,
        TimeoutHost = 8,
        Reversed = 9,
        Refunded = 10,
        Chargeback = 11,
        Dispute = 12,
        DisputeResolved = 13,
        Retrieval = 14,
        AuthSuccess = 15,
        CaptureSuccess = 16,
        TransactionCancelled = 17,
        PartialRefund = 18,
        PendingHostPaymentReply = 30,
        PendingHostReversalReply = 31,
        ProcessingHostReply = 32,
        PendingHostRefundReply = 33,
        PendingCapture = 34,
        BlacklistedCard = 40,
        BlacklistedCardRange = 41,
        BlacklistedName = 42,
        BlacklistedBuyerIP = 43,
        BlacklistedSellerIP = 44,
        BlacklistedEmail = 45,
        BlacklistedPhone = 46,
        ExceededCountVelocity = 47,
        ExceededAmountVelocity = 48,
        CardholderNameExceededDeclinedFrequency = 49,
        OutofPerTxnAmtLimit = 50,
        DeclinedbyExtFDS = 51,
        BINAndIPAndIATAMismatch = 53,
        InvalidBINOrIPAndIATAMismatch = 54,
        InvalidIATA = 55,
        InvalidIPAndIATA = 56,
        InvalidBINAndIPAndIATA = 58,
        BINAndIPAndIATAMismatchWithTimeframe = 59,
        InvalidBINOrIPAndIATAMismatchWithTimeframe = 60,
        InvalidIATAOrIPAndBINMatchWithTimeframe = 61,
        InvalidIPAndIATAWithTimeframe = 62,
        InvalidIPAndBINWithTimeframe = 63,
        Reversalfailed = 64,
        FailedStorageofRequest = 906,
        InvalidMerchant = 2901,
        InvalidRequestData = 2906,
        InvalidHostReply = 2907,
        LateHostReply = 2908,
        InvalidHost = 2911,
        InternalError = 2999
    }

    public enum TXN_Type
    {
        SALE = 3,
        REVERSAL = 13,
        RSALE = 14, //Added by Alex Ng Nkh, 27/10/2021, 28 Aug 2022, MyDebit CNP (NKH001CNP)
        CAPTURE = 15 //Added by Alex Ng Nkh, 19/01/2022, 31 Aug 2022, MyDebit CNP (NKH002CNP)
    }

    public const string C_TXN_SUCCESS_0 = "Transaction Successful";
    public const string C_TXN_FAILED_1 = "Transaction Failed";
    public const string C_TXN_PENDING_2 = "Transaction Pending"; // Added by Jeff, 22 Sept 2013
    public const string C_TXN_REVERSED_9 = "Transaction Reversed"; // Added by Jeff, 22 Sept 2013
    public const string C_TXN_EXPIRED_6 = "Transaction Expired"; // Added by Jeff, 22 Sept 2013
    public const string C_COM_TIMEOUT_5 = "Timeout Out Failed";
    public const string C_FAILED_HOTCARD_9 = "Hot Card / Hot Name";
    public const string C_PENDING_RESP_30 = "Transaction Pending For Response";
    public const string C_QUERY_HOST_33 = "Querying Host";
    public const string C_FAILED_TO_SEND_46 = "Transaction Failed To Send";

    public const string
        C_EXCEED_PER_TXN_AMT_LIMIT_50 =
            "Out of Per Txn Amt Limit"; //Added by Jeff, 16 Jan 2014. Modified by Jeff, 6 Feb 2018, changed from "Exceeded" to "Out of"

    public const string C_TXN_NOT_FOUND_21 = "Transaction Not Exists";
    public const string C_DB_UNKNOWN_ERROR_901 = "Failed to Connect DB";
    public const string C_INVALID_HOST_902 = "Failed to Get Host Information";
    public const string C_FAILED_SET_TXN_STATUS_905 = "Failed to Set Transaction Status";
    public const string C_FAILED_STORE_TXN_906 = "Failed to Store Transaction"; // Failed bInsertNewTxn
    public const string C_FAILED_UPDATE_TXN_RESP_907 = "Failed to Update Transaction Response";
    public const string C_FAILED_FORM_REQ_914 = "Failed to Form Message";
    public const string C_FAILED_COM_INIT_917 = "Failed to Initiate Communications";
    public const string C_INVALID_MID_2901 = "Invalid Service ID"; // Failed bGetMerchant in bInitPayment
    public const string C_COMMON_INVALID_FIELD_2902 = "Invalid Field";
    public const string C_COMMON_MISSING_ELEMENT_2903 = "Missing Required Field"; // Failed bPaymentMain
    public const string C_INVALID_INPUT_2906 = "Invalid Input"; // Failed bCheckUniqueTxn, bInitPayment
    public const string C_INVALID_HOST_REPLY_2907 = "Invalid Host Reply";
    public const string C_INVALID_SIGNATURE_2909 = "Invalid Hash Value";
    public const string C_LATE_HOST_REPLY_2908 = "Late Host Reply";

    public const string
        C_COMMON_UNKNOWN_ERROR_2999 =
            "Internal Error"; //Failed to GenTxnID, bInsertTxnResp, bCheckUniqueTxn, bInsertNewTxn, bProcessSale, bInitPayment

    public const string C_DEFAULT_LANGUAGE_CODE = "EN"; //Added by Jeff, 26 Jul 2013

    public const string
        C_3DESAPPKEYENC =
            "794DEDEC2C6A538C597536C8FCEA66238568466993F50D54EED147D841AA6F60"; //Card data key encrypted by KEK retrieved from DB

    public const string
        C_3DESAPPKEY2 =
            "4648245271298991EDA6FCDF66FE4CCB60ADC6BDA0C09DBB"; //Added by Jeff, 11 Oct 2017, to encrypt/decrypt non card data

    public const string C_3DESAPPVECTOR = "0000000000000000"; //Added by Jeff, 19 Nov 2013
    public const string C_TXN_DECLINED_BY_FDS_400 = "Declined by ExtFDS Rules"; //Added by Nick, 18 apr 2016  fraudwall

    public const string
        C_TXN_DECLINED_BY_FDS_299 = "Declined by ExtFDS Fraud Score"; //Added by Nick, 18 apr 2016  fraudwall

    public const string C_TXN_CANCELLED_17 = "Buyer cancelled"; //Added by HeanSeng, 27 Mar 2018
    public const string C_ACQBIN_NOT_MATCH_CTYPE = "None of the AcqBin matches card type"; //Added by Jawad, 7 Sep 2018
    public const string C_MPI_NOT_ALLOWED = "MPI not allowed"; //Added by Jawad, 7 Jan 2019
    public const string C_MPI_TERMINAL_NOT_FOUND = "No MPI terminal info found"; //Added by Jawad, 27 Dec 2016
    public const string C_FAILED_REVERSAL_65 = "Failed reversal";

    public static string C_3DESAPPKEY; // Added by Jeff, 11 Oct 2017

    public struct STPayLoad // Added by LeeLin, 13 May 2019, GrabPay
    {
        public int time_since_epoch;
        public string sig;
    }
}