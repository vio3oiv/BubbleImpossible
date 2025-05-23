
# BubbleImpossible
![KakaoTalk_20250318_161051647_01](https://github.com/user-attachments/assets/f604b9ae-d2d6-4235-9ec2-54d7cec2d44e)

> 2D 액션 슈팅 게임 – 풍선(HP) 시스템과 특수 스킬로 맵을 클리어하는 것이 목표!

## 개요 (Overview)
**BubbleImpossible**는 플레이어가 풍선(HP)으로 생명을 관리하며,  
적들과 장애물을 돌파하는 2D 액션 슈팅 게임입니다.  
1초 무적, 특수 스킬로 맵 전체 적 처치,  
위아래로 움직이는 장애물 등 다양한 요소로 스릴 넘치는 플레이가 가능합니다.

---

## 주요 특징 (Features)

1. **풍선(HP) 시스템**  
   - 플레이어 체력이 감소할 때마다 풍선 오브젝트가 사라지고, 폭발 이펙트가 표시됩니다.  
   - 풍선이 모두 사라지면 사망 → 게임 오버.

2. **특수 스킬 (X 키)**  
   - 최대 3번 사용 가능.  
   - 사용 시 1초 무적 + 맵 전체 적 처치!  
   - UI(아이콘)로 사용 가능 횟수를 표시하며, 스킬을 쓰면 다른 아이콘(used)으로 변경.

3. **위아래로 움직이는 장애물**  
   - 플레이어가 닿으면 체력 1 감소.  
   - 간단한 패턴이지만 게임에 긴장감을 더해줍니다.

4. **적(Enemy)**  
   - 일반 적: 왼쪽으로 이동하며 탄에 맞으면 HP 감소 후 사망 애니메이션.  
   - SpecialBird: 특정 위치까지 이동 후 멈추고, 3초마다 특수 탄(왼쪽 방향) 발사.  
   - 사망 애니메이션이 재생 중이거나 `isDying` 상태면 플레이어와 충돌해도 데미지 없음.

5. **무적 및 사망 로직**  
   - 플레이어가 피격 당하면 1초간 무적 상태(추가 데미지 무효화).  
   - HP가 0이면 즉시 사망 애니메이션이 재생되고, 3초 뒤 게임 오버.

6. **오프닝·엔딩 씬**  
   - 오프닝: 여러 이미지를 5초씩 표시하며, 2초간 디졸브(페이드) 전환.  
   - 엔딩: 모든 패턴(적)을 처치하면 엔딩 씬이 재생, 게임 클리어.

---

## 플레이 방법 (How to Play)

- **이동**: 방향키(←↑↓→) 혹은 WASD  
- **공격**: Z 키  
- **특수 스킬**: X 키 (1초 무적 + 맵 전역의 적 처치 / 최대 3회)  
- **HP(풍선)**: 적이나 장애물에 부딪히면 1 감소, 0이면 사망  
- **장애물**: 위아래로 움직이는 트랩, 닿으면 HP 감소  

---

## 설치 및 실행 (Installation & Run)
https://vio3oiv.github.io/BubbleImpossible/
---

## 기여 (Contributing)

- 현재 팀 내부 개발용 프로젝트  
- 버그 제보나 제안은 **Issues 탭**을 통해 공유해주세요

---

## 라이선스 (License)

- [MIT License](https://opensource.org/licenses/MIT) 

---

### 문의 (Contact)

- 개발자: [IO3OI,kanghae]  
- 연락처: [ad160515@gmail.com]



