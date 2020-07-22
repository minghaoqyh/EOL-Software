Module CalibConstants

#Region "Common Helios"
    Public Const MaxRgbCur As Double = 5
#End Region
#Region "Vlambda (lambda)"
    'values from "Vlambda approximation; vlambda=f(lambda).xlsx"
    Public Const VlambdaRedA3 As Double = 0.00000139626859302442
    Public Const VlambdaRedA2 As Double = -0.00254056162872413
    Public Const VlambdaRedA1 As Double = 1.52814565233267
    Public Const VlambdaRedA0 As Double = -303.248761568456
    Public Const VlambdaGreA3 As Double = 0.000000793445007949394
    Public Const VlambdaGreA2 As Double = -0.00153699502824626
    Public Const VlambdaGreA1 As Double = 0.972588640018523
    Public Const VlambdaGreA0 As Double = -200.995339960511
    Public Const VlambdaBluA3 As Double = 0.000000751868114377886
    Public Const VlambdaBluA2 As Double = -0.000982810971881889
    Public Const VlambdaBluA1 As Double = 0.42948795366382
    Public Const VlambdaBluA0 As Double = -62.7262353310072
#End Region

#Region "DeltaLambdaDom (T)"
    ''values from "CxCy_of_Temp.xlsx" or data sheet
    'Public Const DeltaLamdaDomTRedOffset As Double = -1.875
    'Public Const DeltaLamdaDomTRedSlope As Double = 0.075
    'Public Const DeltaLamdaDomTGreOffset As Double = 0
    'Public Const DeltaLamdaDomTGreSlope As Double = 0
    ''unclear why these values were used, the other values seem to make more sense
    ''Public Const DeltaLamdaDomTBluOffset As Double = -1.296296296296
    ''Public Const DeltaLamdaDomTBluSlope As Double = 0.051851851851
    ''I guess the other values were used, because they worked
    'Public Const DeltaLamdaDomTBluOffset As Double = -1.2254901967843
    'Public Const DeltaLamdaDomTBluSlope As Double = 0.0491960784313
#End Region
#Region "Phi (T)"

    Public Const CalPhiTRedX0 As Double = 1.18420994281769
    Public Const CalPhiTRedX1 As Double = -0.00736820977181196
    Public Const CalPhiTRedX2 As Double = 0
    Public Const CalPhiTRedX3 As Double = 0
    Public Const CalPhiTGreX0 As Double = 0.98814809
    Public Const CalPhiTGreX1 As Double = 0.00040478
    'todo why the wrong values are used at this place????????
    Public Const CalPhiTGreX0Special As Double = 1.035
    Public Const CalPhiTGreX1Special As Double = -0.0015
    Public Const CalPhiTGreX2 As Double = 0.00000021285
    Public Const CalPhiTGreX3 As Double = -0.0000000837633
    Public Const CalPhiTBluX0 As Double = 1.01315784454346
    Public Const CalPhiTBluX1 As Double = -0.000526315823663026
    Public Const CalPhiTBluX2 As Double = 0
    Public Const CalPhiTBluX3 As Double = 0
#End Region
#Region "T junction (Rth,eta)"
    Public Const CalTempChipRthRed As Double = 1.8
    Public Const CalTempChipEtaRed As Double = 0.19
    Public Const CalTempChipRthGre As Double = 1.4
    Public Const CalTempChipEtaGre As Double = 0.27
    Public Const CalTempChipRthBlu As Double = 1.4
    Public Const CalTempChipEtaBlu As Double = 0.37
#End Region
#Region "Cx/Cy (I)"
    Public Const CalCxCyIBorder As Double = 10
#End Region
#Region "Cx/Cy red"
    Public Const CalCxTLowRedX0 As Double = -0.0033102300949394
    Public Const CalCxTLowRedX1 As Double = 0.00013240899716038
    Public Const CalCxTLowRedX2 As Double = 0
    Public Const CalCxTLowRedX3 As Double = 0
    Public Const CalCxTHighRedX0 As Double = -0.0033102300949394
    Public Const CalCxTHighRedX1 As Double = 0.00013240899716038
    Public Const CalCxTHighRedX2 As Double = 0
    Public Const CalCxTHighRedX3 As Double = 0
    Public Const CalCxTBorderRed As Double = 150

    Public Const CalCyTLowRedX0 As Double = 0.0032948900479823
    Public Const CalCyTLowRedX1 As Double = -0.00013179599773138
    Public Const CalCyTLowRedX2 As Double = 0
    Public Const CalCyTLowRedX3 As Double = 0
    Public Const CalCyTHighRedX0 As Double = 0.0032948900479823
    Public Const CalCyTHighRedX1 As Double = -0.00013179599773138
    Public Const CalCyTHighRedX2 As Double = 0
    Public Const CalCyTHighRedX3 As Double = 0
    Public Const CalCyTBorderRed As Double = 150
#End Region
#Region "Cx/Cy green"
    Public Const CalCxTLowGreX0 As Double = -0.002546879928559
    Public Const CalCxTLowGreX1 As Double = 0.00010187499719904
    Public Const CalCxTLowGreX2 As Double = 0
    Public Const CalCxTLowGreX3 As Double = 0
    Public Const CalCxTHighGreX0 As Double = -0.002546879928559
    Public Const CalCxTHighGreX1 As Double = 0.00010187499719904
    Public Const CalCxTHighGreX2 As Double = 0
    Public Const CalCxTHighGreX3 As Double = 0
    Public Const CalCxTBorderGre As Double = 150

    Public Const CalCyTLowGreX0 As Double = -0.0016111100558191
    Public Const CalCyTLowGreX1 As Double = 0.000064444400777574
    Public Const CalCyTLowGreX2 As Double = 0
    Public Const CalCyTLowGreX3 As Double = 0
    Public Const CalCyTHighGreX0 As Double = 0.00417958991602
    Public Const CalCyTHighGreX1 As Double = -0.000142931996379
    Public Const CalCyTHighGreX2 As Double = 0.00000231223998525
    Public Const CalCyTHighGreX3 As Double = -0.0000000121088001847
    Public Const CalCyTBorderGre As Double = 50
#End Region
#Region "Cx/Cy blue"
    Public Const CalCxTLowBluX0 As Double = 0.0011555600212886
    Public Const CalCxTLowBluX1 As Double = -0.000046222201490309
    Public Const CalCxTLowBluX2 As Double = 0
    Public Const CalCxTLowBluX3 As Double = 0
    Public Const CalCxTHighBluX0 As Double = 0.0011555600212886
    Public Const CalCxTHighBluX1 As Double = -0.000046222201490309
    Public Const CalCxTHighBluX2 As Double = 0
    Public Const CalCxTHighBluX3 As Double = 0
    Public Const CalCxTBorderBlu As Double = 150

    Public Const CalCyTLowBluX0 As Double = -0.0011914799688383
    Public Const CalCyTLowBluX1 As Double = 0.000047659101255703
    Public Const CalCyTLowBluX2 As Double = 0
    Public Const CalCyTLowBluX3 As Double = 0
    Public Const CalCyTHighBluX0 As Double = -0.0011914799688383
    Public Const CalCyTHighBluX1 As Double = 0.000047659101255703
    Public Const CalCyTHighBluX2 As Double = 0
    Public Const CalCyTHighBluX3 As Double = 0
    Public Const CalCyTBorderBlu As Double = 150
#End Region
#Region "Phi Max"
    Public Const CalPhiMaxRed As Double = 400
    Public Const CalPhiMaxGre As Double = 1150
    Public Const CalPhiMaxBlu As Double = 60
#End Region

#Region "CalCheck"
    Public Const CalIndexCur As Integer = 0
    Public Const CalIndexVol As Integer = 2
    Public Const CalIndexDac As Integer = 4
    Public Const CalIndexCx As Integer = 6
    Public Const CalIndexCy As Integer = 24
    Public Const CalIndexTempChip As Integer = 42
    Public Const CalIndexPhi As Integer = 44
    Public Const CalIndexPhiAdc As Integer = 72
    Public Const CalIndexPhiMax As Integer = 90
    Public Const CalIndexAdditional As Integer = 100
    Public Const CalIndexDacMax As Integer = 101
    Public Const CalIndexIByDac As Integer = 102
    Public Const CalIndexUByI As Integer = 106
    Public Const CalIndexSuperR9 As Integer = 110
    Public Const CalIndexAfterLast As Integer = 112
#End Region

#Region "Integration Times"
    Public Const IntTimeRed1 As Double = 4.8
    Public Const IntTimeRed2 As Double = 4.8
    Public Const IntTimeRed3 As Double = 4.8
    Public Const IntTimeRed4 As Double = 4.8
    Public Const IntTimeRed5 As Double = 2.4
    Public Const IntTimeRed6 As Double = 2.4
    Public Const IntTimeRed7 As Double = 1.6
    Public Const IntTimeRed8 As Double = 1.6
    Public Const IntTimeRed9 As Double = 1.6
    Public Const IntTimeRed10 As Double = 1.6
    Public Const IntTimeRed11 As Double = 0.95
    Public Const IntTimeRed12 As Double = 0.95
    Public Const IntTimeRed13 As Double = 0.95
    Public Const IntTimeRed14 As Double = 0.95
    Public Const IntTimeRed15 As Double = 0.95
    Public Const IntTimeRed16 As Double = 0.95

    Public Const IntTimeGre1 As Double = 4.8
    Public Const IntTimeGre2 As Double = 4.8
    Public Const IntTimeGre3 As Double = 4.8
    Public Const IntTimeGre4 As Double = 4.8
    Public Const IntTimeGre5 As Double = 2.4
    Public Const IntTimeGre6 As Double = 2.4
    Public Const IntTimeGre7 As Double = 2.4
    Public Const IntTimeGre8 As Double = 1.4
    Public Const IntTimeGre9 As Double = 1.4
    Public Const IntTimeGre10 As Double = 1.4
    Public Const IntTimeGre11 As Double = 1.4
    Public Const IntTimeGre12 As Double = 0.95
    Public Const IntTimeGre13 As Double = 0.95
    Public Const IntTimeGre14 As Double = 0.95
    Public Const IntTimeGre15 As Double = 0.95
    Public Const IntTimeGre16 As Double = 0.95

    Public Const IntTimeBlu1 As Double = 5
    Public Const IntTimeBlu2 As Double = 5
    Public Const IntTimeBlu3 As Double = 5
    Public Const IntTimeBlu4 As Double = 2.4
    Public Const IntTimeBlu5 As Double = 2.4
    Public Const IntTimeBlu6 As Double = 2.4
    Public Const IntTimeBlu7 As Double = 1.4
    Public Const IntTimeBlu8 As Double = 1.4
    Public Const IntTimeBlu9 As Double = 1.4
    Public Const IntTimeBlu10 As Double = 1.4
    Public Const IntTimeBlu11 As Double = 1.4
    Public Const IntTimeBlu12 As Double = 0.9
    Public Const IntTimeBlu13 As Double = 0.9
    Public Const IntTimeBlu14 As Double = 0.9
    Public Const IntTimeBlu15 As Double = 0.9
    Public Const IntTimeBlu16 As Double = 0.9
#End Region

End Module
