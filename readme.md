https://github.com/nesrak1/UABEA.git

기반으로 대충 만든 피닉스 포인트 한국어 패치 툴
내가 게으르고 책임감없는 사람이라 이거 계속 붙잡고 사후지원 안할거 같아서 그냥 코드 공개해놓음
일단 버그는 이슈에 올려주면 시간 날때 수정하긴 할텐데 얼마나 갈지는 모르겠음

 - Phoenix Point가 설치된 폴더에 PhoenixPointKoreanPatcher 폴더를 만들고 패치툴을 복사해 넣습니다.
 - https://docs.google.com/spreadsheets/d/1rYYCY0szEZN_xvBV8lEfpWpefZ9b66lL1U3bIAQFdSQ/edit?usp=sharing 들어가서 댓글로 번역 제안
  -- I2Languages.tsv
  -- I2Languages_GeoEvents.tsv
  -- I2Languages_GeoHavens.tsv
  -- I2Languages_Research.tsv
  -- I2Languages_CharNames.tsv // 케릭터 이름인데 번역 안해도 됨
  -- I2Languages_Mod.tsv       // 뭐에 쓰는지는 모르겠는데 문자열 들어가있는게 없음
  -- I2Languages_Saber.tsv     // DLC5랑 관계있는 파일같은데 2021.10.20일 기준으로 문자열 들어가있는게 없음
 - 구글 시트의 각 탭을 파일 > 다운로드 > .tsv 로 저장한다
  -- "피닉스 포인트 번역 - I2Languages.tsv" 이런식으로 저장되면 "피닉스 포인트 번역 - " 부분은 지우고 PhoenixPointKoreanPatcher 폴더에 넣는다.
 - PhoenixPointKoreanPatcher.exe 실행
 - PhoenixPointKoreanPatcher 폴더에 sharedassets0.assets 파일이 생기면 PhoenixPointWin64_Data에 덮어 쓴다.
 - 게임 실행 ㄱㄱ
 - 옵션에서 중국어 선택
  -- 중국몽이 이니 조선족이니 하면서 어그로 끄는 새끼가 있는데 한국어랑 중국어가 둘다 2바이트 문자라 중국어에 번역문 덮어쓴거임 헛소리 ㄴㄴ
 - 피닉스 포인트 업데이트 후 한글 안나오면 PhoenixPointKoreanPatcher.exe 실행 해주면 나올거임
  -- 그래도 한글 안나오는거는 영어 원문이 바껴서 다시 번역해야 되거나 새로 추가된 문장이니 추가 번역 요청하센
  -- 근대 내가 그때까지 계속 총대 매고 있다는 보장이 없으니 요청해보고 답장 없으면 I2Languages.tsv 열어서 영어 원문 옆에 "번역문" 이라고 박혀있는거 번역문으로 교체하고 PhoenixPointKoreanPatcher.exe 실행하면 번역 추가되니깐 그렇게 하센
