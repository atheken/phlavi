#PHLAVI
======

REST API for assessed property information in the City of Philadelphia.

<pre>
GET /account/883525510
GET /address/908-14%20N%2003RD%20ST

[
    {
        "Acct_Num": "883525510",
        "Address": "908-14 N 03RD ST",
        "Unit": "",
        "Homestead_Ex": false,
        "Prop_Cat": "COMMERCIAL",
        "Prop_Type": "OFFICE AND/OR HOTELS",
        "Num_Stor": 0,
        "Mktval_14": 355200,
        "LandVal_14": 63900,
        "ImpVal_14": 291300,
        "Abat_Ex_14": 81122,
        "Mktval_13": 200000,
        "LandVal_13": 26200,
        "ImpVal_13": 173800,
        "Abat_Ex_13": 48400,
        "Latitude": 39.9654,
        "Longitude": -75.142601
    }
]

GET /account/change/883525510
GET /address/change/908-14%20N%2003RD%20ST

[
    {
        "Acct_Num": "883525510",
        "Address": "908-14 N 03RD ST",
        "Unit": "",
        "Values": {
            "Market_Change": 155200,
            "Land_Change": 37700,
            "Improvement_Change": 117500,
            "Abatement_Change": 32722
        }
    }
]
</pre>
