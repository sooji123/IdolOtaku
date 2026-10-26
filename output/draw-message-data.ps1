Add-Type -AssemblyName System.Drawing
$bmp = New-Object System.Drawing.Bitmap 2000,1400
$g = [System.Drawing.Graphics]::FromImage($bmp)
$g.Clear([System.Drawing.Color]::White)
$g.SmoothingMode = 'AntiAlias'
$g.TextRenderingHint = 'AntiAliasGridFit'
$orange = [System.Drawing.ColorTranslator]::FromHtml('#EA761F')
$dark = [System.Drawing.ColorTranslator]::FromHtml('#252A31')
$muted = [System.Drawing.ColorTranslator]::FromHtml('#69717C')
$pale = [System.Drawing.ColorTranslator]::FromHtml('#FFF3E9')
$pen = New-Object System.Drawing.Pen $orange,3
$pen.CustomEndCap = New-Object System.Drawing.Drawing2D.AdjustableArrowCap 5,6
$border = New-Object System.Drawing.Pen ([System.Drawing.ColorTranslator]::FromHtml('#E3E5E8')),2
$font = New-Object System.Drawing.Font 'Malgun Gothic',17
$small = New-Object System.Drawing.Font 'Malgun Gothic',14
$head = New-Object System.Drawing.Font 'Consolas',21,([System.Drawing.FontStyle]::Bold)
$title = New-Object System.Drawing.Font 'Malgun Gothic',32,([System.Drawing.FontStyle]::Bold)
function Txt($s,$x,$y,$f=$font,$c=$dark) { $g.DrawString($s,$f,(New-Object System.Drawing.SolidBrush $c),[single]$x,[single]$y) }
function Box($x,$y,$w,$h,$name,$lines) {
 $g.FillRectangle((New-Object System.Drawing.SolidBrush $pale),$x,$y,$w,58)
 $g.DrawRectangle($border,$x,$y,$w,$h)
 $g.FillRectangle((New-Object System.Drawing.SolidBrush $orange),$x,$y,5,58)
 Txt $name ($x+20) ($y+12) $head
 $yy=$y+76
 foreach($line in $lines) { Txt $line ($x+20) $yy $small; $yy+=32 }
}
function Arrow($points) {
 $pts = @(); foreach($pt in $points) { $pts += New-Object System.Drawing.Point $pt[0],$pt[1] }
 $g.DrawLines($pen,[System.Drawing.Point[]]$pts)
}
Txt '아이돌 메시지 · 데이터 조회 및 가공 구조' 75 45 $title
Txt '테이블 조회 → 메시지 유형 판별 → 캐릭터별 대화 구조 생성' 78 112 $font $muted
Box 75 195 550 165 'IdolTableSO' @('dataList : List<Idol>','캐릭터 ID 및 기본 정보')
Box 725 195 550 165 'DialogueTableSO' @('dataList : List<Dialogue>','ID / Text / ChoiceStrings / Dialogues / Function')
Box 1375 195 550 165 'DialogueStringTableSO' @('SetLanguage(languageType) / GetString(key)','언어별 문구를 Dictionary<string, string>으로 구성')
Box 660 440 680 205 'DataManager' @('UpdateDB() : TableCenter에서 테이블 조회','아이돌 목록 + 대화 테이블을 DialogueDBSO에 전달','GetDialogueString(key) : 현재 언어의 문구 조회')
Arrow @(@(350,360),@(350,410),@(800,410),@(800,440))
Arrow @(@(1000,360),@(1000,440))
Arrow @(@(1650,360),@(1650,410),@(1200,410),@(1200,440))
Txt '조회 · 언어 설정' 1400 422 $small $muted
Box 660 725 680 225 'DialogueDBSO' @('BuildDialogueDataList() : 테이블 행 → MessageData','idolId = ID / 1000, branchId = ID % 1000','MergeDatas() : 같은 캐릭터 ID의 정보와 대화 연결','SetUpSaveData() : 진행 기록 및 미선택(-1) 초기화')
Arrow @(@(1000,645),@(1000,725))
Txt 'UpdateDB(idolList, dialogueTable)' 1030 670 $small $muted
Box 75 1050 420 250 'CharacterDialogueData' @('ID : int','characterData : Idol','dialogueData : DialogueData','캐릭터 정보 + 대화 데이터')
Box 550 1050 420 250 'DialogueData / Branch' @('DialogueData.branches : List<Branch>','Branch.messageDatas : List<MessageData>','Branch ID별 메시지 묶음 생성','선택지에 choiceID 순차 부여')
Box 1025 1050 440 250 'MessageData' @('branchID / messageType / choiceID','chat / imageID / choices','유형 판별 우선순위','Choice → Image → Event → Chat')
Box 1520 1050 405 250 'DialogueSaveData' @('lastBranch / lastID / maxBranch','alarm / BranchSaveDatas','캐릭터별 진행 상태 관리','선택 결과 초기값 : -1')
Arrow @(@(800,950),@(800,995),@(285,995),@(285,1050))
Arrow @(@(900,950),@(900,1018),@(760,1018),@(760,1050))
Arrow @(@(1100,950),@(1100,1018),@(1245,1018),@(1245,1050))
Arrow @(@(1200,950),@(1200,995),@(1720,995),@(1720,1050))
Txt '화살표: 데이터 조회·변환 흐름   |   하단: 생성·구성되는 데이터 클래스   |   UI 출력 클래스 제외' 80 1340 $small $muted
$dest=Join-Path $PSScriptRoot 'idol-message-data-diagram.png'
$bmp.Save($dest,[System.Drawing.Imaging.ImageFormat]::Png)
$g.Dispose(); $bmp.Dispose()
Write-Output $dest
