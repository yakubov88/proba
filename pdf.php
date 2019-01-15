<?php



class PDF extends tFPDF
{

    function __construct()
    {
        parent::__construct();
    }



    //Cell with horizontal scaling if text is too wide
    function CellFit($w, $h=0, $txt='', $border=0, $ln=0, $align='', $fill=false, $link='', $scale=false, $force=true)
    {
        //Get string width
        $str_width=$this->GetStringWidth($txt);

        //Calculate ratio to fit cell
        if($w==0)
            $w = $this->w-$this->rMargin-$this->x;
        $ratio = ($w-$this->cMargin*2)/$str_width;

        $fit = ($ratio < 1 || ($ratio > 1 && $force));
        if ($fit)
        {
            if ($scale)
            {
                //Calculate horizontal scaling
                $horiz_scale=$ratio*100.0;
                //Set horizontal scaling
                $this->_out(sprintf('BT %.2F Tz ET',$horiz_scale));
            }
            else
            {
                //Calculate character spacing in points
                $char_space=($w-$this->cMargin*2-$str_width)/max($this->MBGetStringLength($txt)-1,1)*$this->k;
                //Set character spacing
                $this->_out(sprintf('BT %.2F Tc ET',$char_space));
            }
            //Override user alignment (since text will fill up cell)
            $align='';
        }

        //Pass on to Cell method
        $this->Cell($w,$h,$txt,$border,$ln,$align,$fill,$link);

        //Reset character spacing/horizontal scaling
        if ($fit)
            $this->_out('BT '.($scale ? '100 Tz' : '0 Tc').' ET');
    }

    //Cell with horizontal scaling only if necessary
    function CellFitScale($w, $h=0, $txt='', $border=0, $ln=0, $align='', $fill=false, $link='')
    {
        $this->CellFit($w,$h,$txt,$border,$ln,$align,$fill,$link,true,false);
    }

    //Patch to also work with CJK double-byte text
    function MBGetStringLength($s)
    {
        if($this->CurrentFont['type']=='Type0')
        {
            $len = 0;
            $nbbytes = strlen($s);
            for ($i = 0; $i < $nbbytes; $i++)
            {
                if (ord($s[$i])<128)
                    $len++;
                else
                {
                    $len++;
                    $i++;
                }
            }
            return $len;
        }
        else
            return strlen($s);
    }

    function Header()
    {

//        $this->AddFont('DejaVu', '', 'DejaVuSansCondensed-Bold.ttf', true);
//        $this->AddFont('DejaVuBold','','DejaVuSans-Bold.ttf',true);




        $this->AddFont('DejaVu','','DejaVuSans.ttf',true);
        $this->AddFont('dejavu_bold','','DejaVuSans-Bold.ttf',true);

                $FontType = 'arial';
                //$this->SetFont($FontType,'B',10);
                $this->SetFont('DejaVu','',9);
                $this->Cell(190,-5,'Page '.$this->PageNo().'       ',0,0,'R',0);
                $this->SetFont('Dejavu','',10);
                $this->Ln( 1 );
                $this->Cell(145,8,"","LT",0,'C',0);
                //$this->Cell(30,8,"F 824.01-09",1,0,'C',0);
                //$this->SetFont('Dejavu','',11);
                //$this->Cell(115,8,"INTEGRATED SYSTEM FORM",1,0,'C',0);

                $this->SetFont($FontType,'B',18);
                $this->Cell(50,8,"","LRT");
                $this->Ln( 8 );
                //$this->SetFont('DejaVu','',10);
                //$this->Cell(30,8,"Revision - 01",1,0,'C',0);
                //$this->SetFont($FontType,'B',11);
                //$this->Cell(115,8,"QUALITY CERTIFICATE",1,0,'C',0);
                $this->Cell(145,8,"","LB",0,'C',0);

                $this->Ln( -4 );
                $this->Cell(145,8,"INSPECTION CERTIFICATE","",0,'C',0);
                $this->Ln( 4 );

                $this->SetFont($FontType,'B',8);
                $this->Cell(145,4,"","",0,'C',0);
                $this->MultiCell(50,4,"EN ISO 9001/BS OHSAS 18001/\nEN ISO 14001",1,'C',0);



                $this->Ln( 4 );

                $now = new DateTime();
                $now = $now->format('d.m.y');
                $this->SetFont($FontType,'B',11);
                $this->Cell(25,7,"CUSTOMER:","LT",0,'L',0);
                //$this->Cell(5,7,"","T",0,'L',0);
                $this->SetFont('Dejavu','',10);

                $this->CellFitScale(95,7,preg_replace('/\s+/S', " ", 'сдсдсдсд'),"T",0,'L',0);
                //$this->Cell(100,7,preg_replace('/\s+/S', " ", $row_data_HI[1]),"T",0,'L',0);


                $this->SetFont($FontType,'B',11);
                $this->Cell(75,7,"INSPECTION CERTIFICATE","TR",0,'L',0);
                $this->SetFont('Dejavu','',10);
                $this->Ln( 5 );
                $this->Cell(25,7,"","L",0,'L',0);
                $this->CellFitScale(95,7,preg_replace('/\s+/S', " ", 'ефггдддд'),"",0,'L',0);
                //$this->Cell(95,7,"ПРОМИШЛЕНО-МОНТАЖНА ГРУПА ШУМЕН ООД","",0,'L',0);
                $this->SetFont($FontType,'B',10);
                //$this->Cell(13,7,"No:","",0,'R',0);
                $this->SetFont('dejavu_bold','',10);
                $this->Cell(13,7,html_entity_decode(utf8_decode("&#8470;")).':',"",0,'R',0);
                $this->SetFont('Dejavu','',10);







                //if(preg_replace('/\s+/S', "", $row_data_HI[12]) == "1") Valeria : It will exists as default 11.06.2018


                //If [Address 2] information of customer exists, then display it.


                $this->Ln( 5 );
                $this->SetFont('Dejavu','',10);
                $this->Cell(25,7,"","L",0,'L',0);





                $this->Ln( 5 );
                $this->Cell(25,7,"","L",0,'L',0);
                $this->Cell(95,7,'вевесдсдс',"",0,'L',0);
                $this->Cell(75,7,"","R",0,'L',0);
                $this->Ln( 7 );
                $this->SetFont($FontType,'B',11);
                $this->Cell(30,7,"PRODUCT","L",0,'L',0);
                $this->Cell(5,7,"","",0,'L',0);
                $this->SetFont('Dejavu','',11);
                $this->Cell(85,7,"","",0,'L',0);
                $this->SetFont($FontType,'B',11);
                $this->Cell(75,7,"ORDER","R",0,'L',0);
                $this->SetFont('Dejavu','',11);
                $this->Ln( 5 );




                    $this->SetFont($FontType,'B',10);
                    $this->Cell(35,7,"Product:","L",0,'R',0);
                    $this->SetFont('Dejavu','',10);
                    $this->CellFitScale(75,7,'дсдсдсдсдсд',"",0,'L',0);
                    $this->SetFont($FontType,'B',10);
                    $this->Cell(38,7,"Customer Order:","",0,'R',0);
                    $this->SetFont('Dejavu','',10);
                    $this->Cell(47,7,preg_replace('/\s+/S', " ", 'едфдффддф'),"R",0,'L',0);
                    $this->Ln( 5 );
                    $this->SetFont($FontType,'B',10);
                    $this->Cell(35,7,"Surface:","L",0,'R',0);
                    $this->SetFont('DejaVu','',10);
                    $this->Cell(75,7,'фдфдфдф',"",0,'L',0);
                    $this->SetFont($FontType,'B',10);
                    $this->Cell(38,7,"Alcomet Contract:","",0,'R',0);
                    $this->SetFont('Dejavu','',10);
                    $this->Cell(47,7,'дсдффддфд',"R",0,'L',0);
                    $this->Ln( 5 );
                    $this->SetFont($FontType,'B',10);
                    $this->Cell(35,7,"Alloy/standard:","L",0,'R',0);
                    $this->SetFont('DejaVu','',10);
                    $this->Cell(75,7,preg_replace('/\s+/S', " ", 'дфдфдфд')." / EN 573-3","",0,'L',0);
                    $this->SetFont($FontType,'B',10);
                    $this->Cell(33,7,"Packing List ","",0,'R',0);
                    $this->SetFont('dejavu_bold','',9);
                    $this->Cell(5,7,html_entity_decode(utf8_decode("&#8470;")).':',"",0,'R',0);
                    $this->SetFont('Dejavu','',10);
                    $this->Cell(47,7,'ффдфдфдфдфд',"R",0,'L',0);
                    $this->Ln( 5 );
                    $this->SetFont($FontType,'B',10);
                    $this->Cell(35,7,"Temper/standard:","L",0,'R',0);
                    $this->SetFont('Dejavu','',10);
                    $this->Cell(75,7,preg_replace('/\s+/S', " ", 'дфдфдфдфдф')." / ". 'фдфдфдфдфдфдфд',"",0,'L',0);
                    $this->SetFont($FontType,'B',10);
                    $this->Cell(33,7,"Ref ","",0,'R',0);
                    $this->SetFont('dejavu_bold','',9);
                    $this->Cell(5,7,html_entity_decode(utf8_decode("&#8470;")).':',"",0,'R',0);
                    $this->SetFont('Dejavu','',10);
                    $this->Cell(47,7,preg_replace('/\s+/S', " ", 'фдфдфдфдфд'),"R",0,'L',0);
                    $this->Ln( 5 );
                    $this->SetFont($FontType,'B',10);
                    $this->Cell(35,7,"Size:","L",0,'R',0);
                    $this->SetFont('DejaVu','',10);
                    $this->Cell(75,7,'дфдфдфдфдффд',"",0,'L',0);
                    $this->SetFont($FontType,'B',10);
                    $this->Cell(38,7,"","",0,'R',0);
                    $this->SetFont('Dejavu','',10);
                    $this->Cell(47,7,"","R",0,'L',0);
                    $this->Ln( 5 );
                    $this->SetFont($FontType,'B',10);
                    $this->Cell(82,7,"Technical condition for inspection and delivery:","L",0,'R',0);
                    $this->SetFont('Dejavu','',10);
                    $this->Cell(28,7,preg_replace('/\s+/S', " ", 'фдфдфдфдфдфд'),"",0,'L',0);
                    $this->SetFont($FontType,'B',10);
                    $this->Cell(85,7,"","R",0,'R',0);
                    $this->Ln( 5 );
                    $this->SetFont($FontType,'B',10);
                    $this->Cell(82,7,"Tolerances on shape and dimension:","LB",0,'R',0);
                    $this->SetFont('Dejavu','',10);
                    $this->Cell(28,7,preg_replace('/\s+/S', " ", 'фдфдфдфдфдфдф'),"B",0,'L',0);
                    $this->SetFont($FontType,'B',10);
                    $this->Cell(38,7,"","B",0,'R',0);
                    $this->SetFont('Dejavu','',10);
                    $this->Cell(47,7,"","RB",0,'L',0);
                    $this->Ln( 15 );




    } // End of Header Function

    function Footer()
    {
        $this->AddFont('DejaVu','','DejaVuSans.ttf',true);
        $this->AddFont('dejavu_bold','','DejaVuSans-Bold.ttf',true);
        $FontType = 'arial';
        $this->SetFont($FontType,'B',10);

        $QualityUserText = "Gabriela Georgieva, Quality Management Department";



        if(0)
        {
            $this->SetY(-16);
            $this->SetY(-16);
            //$this->Image('Certification\Images\sign.png',150,250,-105);
            $this->Image('Certification\Images\quality stamp.jpg',175,251,-255);
            $this->MultiCell(190,5,"ALCOMET AD - II Industrial Zone - Shumen 9700 - Bulgaria - Tel. (+359) 54/858601 - Fax (+359) 54/858688\ne-mail office@alcomet.eu, www.alcomet.eu\nVAT: BG837066358",0,'C',0);
            $this->SetY(-16);
            $this->SetFont($FontType,'',10);
            $this->MultiCell(190,-8,$QualityUserText,0,'R',0);
            $this->SetY(-32);
            $this->SetFont($FontType,'',9);
            $this->MultiCell(190,4,"We hereby certify the material described above has been tested and complies with the terms of Contract (Order).\nThis inspection certificate has been issued digitally and is valid without signature.",0,'L',0);
        }
        else
        {
            $this->SetY(-16);
            $this->MultiCell(190,5,"ALCOMET AD - II Industrial Zone - Shumen 9700 - Bulgaria - Tel. (+359) 54/858601 - Fax (+359) 54/858688\ne-mail office@alcomet.eu, www.alcomet.eu\nVAT: BG837066358",0,'C',0);
            $this->SetY(-16);
            $this->SetFont('DejaVu','',10);
            $this->MultiCell(190,-8,$_SESSION['name'],0,'R',0);
            $this->SetY(-32);
            $this->SetFont($FontType,'',9);
            $this->MultiCell(190,4,"We hereby certify the material described above has been tested and complies with the terms of Contract (Order).\nThis inspection certificate has been issued digitally and is valid without signature.",0,'L',0);
        }
    } // End of Footer Function
} //End of FPDF Class

$pdf = new PDF();


$pdf->AddPage();

$FontType = 'arial'; //"arial","times","courier","helvetica","symbol"
$page_height = 279.4;

$pdf->SetFont($FontType,'B',9);
$pdf->Cell(25,6,'мдсдсдсдс',1,0,'C',0);
$pdf->Cell(150,6,"Packages",1,0,'C',0);
$pdf->Cell(20,6,"Qty. (KG)",1,0,'C',0);
$pdf->SetFont($FontType,'',8);
$pdf->AddFont('DejaVu','','DejaVuSans.ttf',true);

$pdf->SetFont('DejaVu','',8);

$pdf->Ln( 6 );

$pdf->Output('pdffile','I');




/*
$row_data_CP_new['packs'] = $row_data_CP_new['packs']." , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333 , PA00695333";
*/








/*
	$PackCoilLineCount = (substr_count($row_data_CP_new['coils'], "\n")+1);
	$PackPartsLineCount = (substr_count($row_data_CP_new['packs'], ",")+1);
	$PackPartsLineCount = floor(($PackPartsLineCount-(0.1))/7)+1;

	if($PackCoilLineCount > $PackPartsLineCount)
	{
		$PackTableLineCount = $PackCoilLineCount;
	}
	else
	{
		$PackTableLineCount = $PackPartsLineCount;
	}
		$pdf->Cell(175,5,"",0,0,'C',0);
		$pdf->MultiCell(20,5,$row_data_CP_new_Qty['quantity'],0,'C',0);
		$pdf->Ln( -5 );

		$pdf->MultiCell(25,5,$row_data_CP_new['coils'],0,'C',0);
		$pdf->Ln( -($PackCoilLineCount*5) );

		$pdf->Cell(25,5,"",0,0,'C',0);
		$pdf->MultiCell(150,5,$row_data_CP_new['packs'],0,'L',0);

		$pdf->Ln( -($PackPartsLineCount*5) );

		$pdf->Cell(25,($PackTableLineCount*5),"",1,0,'C',0);
		$pdf->Cell(150,($PackTableLineCount*5),"",1,0,'C',0);
		$pdf->Cell(20,($PackTableLineCount*5),"",1,0,'C',0);
		$pdf->Ln( ($PackTableLineCount*5) );

*/


/*
	$pdf->Cell(25,30,preg_replace('/\s+/S', " ", $row),1,0,'C',0);
	$pdf->MultiCell(150,6,preg_replace('/\s+/S', " ", $data_coilpart_new),0,'L',0);
	$pdf->Ln( -30 );
	$pdf->Cell(175,30,"",1,0,'C',0);
	$pdf->Cell(20,30,number_format((float)$row_data_CP_new_Qty[$row], 0, '', '.') ,1,0,'C',0);
	$pdf->Ln( 30 );*/






?>
