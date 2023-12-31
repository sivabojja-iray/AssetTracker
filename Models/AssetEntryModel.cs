﻿using iTextSharp.text.pdf.security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I_RAY_ASSET_TRACKER_MVC.Models
{
    public class AssetEntryModel
    {
        public string Team { get; set; }
        public string HWSWname { get; set; }
        public string HWSWValidatedby { get; set; }
        public string HWSWDescriptionCategory { get; set; }
        public string SerialNumberVersionNumber { get; set; }
        public string AssetType { get; set; }
        public string DateofReciept { get; set; }
        public string OwnerShip { get; set; }
        public string HWSWVerifiedBy { get; set; }
        public string HWSWVerifiedValidatedDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string Remarks { get; set; }
        public string PalpatineVersion { get; set; }
        public string MacAddressSV { get; set; }
        public string HIProgram { get; set;}
        public string IMEINo { get; set;}
        public string DateofVersionUpdated { get; set; }
        public string PreviousOS { get; set; }
        public string ModelIdentifier { get; set; }
        public string OSVerificationDate { get; set; }
        public string DateofPurchase { get; set; }
        public string RecordUpdatedby { get; set; }
        public string RecordUpdatedDate { get; set;}
        public bool yes { get; set; }
        public bool no { get; set; }
        public bool Hardware { get; set; }
        public bool Software { get; set; }
        public string AssetListfileupload { get; set; }
        public string ConsumableBulkEntryTeam { get; set; }
        public string ConsumableBulkEntryHWSWname { get; set; }
        public string ConsumableBulkEntryHWSWValidatedby { get; set; }
        public string ConsumableBulkEntryHWSWDescriptionCategory { get; set; }
        public string ConsumableBulkEntryQuantity { get; set; }
        public string ConsumableBulkEntryAssetType { get; set; }
        public string ConsumableBulkEntryDateofReciept { get; set; }
        public bool ConsumableBulkEntryyes { get; set; }
        public bool ConsumableBulkEntryno { get; set; }
        public string ConsumableBulkEntryHWSWVerifiedBy { get; set; }
        public string ConsumableBulkEntryHWSWVerifiedValidatedDate { get; set; }
        public string ConsumableBulkEntryInvoiceNumber { get; set; }
        public string ConsumableBulkEntryRemarks { get; set; }
        public string ConsumableBulkEntryRecordUpdatedby { get; set; }
        public string ConsumableBulkEntryRecordUpdatedDate { get; set; }
    }
}