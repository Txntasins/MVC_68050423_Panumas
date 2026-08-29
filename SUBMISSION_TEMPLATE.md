# SUBMISSION - Exit Exam MVC 1/2569 (เสาร์บ่าย)

## 1. วิธีเปิดโปรแกรม
- ภาษา/เฟรมเวิร์ก: ภาษา C# (.NET) + Avalonia UI(Desktop GUI)
- Entry point / คำสั่งเปิดโปรแกรม: รวมคำสั่งเปิดโปรแกรมเปิดผ่าน terminal
        cd FFCR
        dotnet restore
        dotnet run
    Emtry point
        Program.cs Main()
        AppBuilder
        App.axaml.cs:onFrameworkInitializationCompleted() ใช้โหลด seed_data.json
    ต้องมี .NET SDK ด้วย
- หมายเหตุที่จำเป็น (ถ้ามี): 

## 2. ตารางเชื่อมโยง Requirements

| Requirement | Model / Domain | Controller / Action | View / Screen |
|---|---|---|---|
| R1 | Model/*(Member, RoleChaneRequest,Decision, enums) + DataStore | Program.cs (entry point), App.axaml.cs | View/MainWindow.axaml |
| R2 | Member, RoleChangeRequest | MemberController.GetAllMembers(); RequestController.createRequest() | แท็บหน้าต่าง Member & Requests |
| R3 | Decision, DecisionResult | DecisionController.GetEligibleDeciders(), DecisionController.Submit() | แท็บหน้าต่าง "Submit Decision" |
| R4 | RoleChangeRequest.Status, Member.Role | DecisionController.Submit() ไปที่ FinalizeIfPossible()| ผลสะท้อนในตาราง Requests/Members หลังจาก Reload |
| R5 | RoleChangeRequest, OperationResult | RequestController.CancelRequest(); SummaryController.GetRequestsByStatus(), GetVoteCounts() | หน้าต่างแท็บ Cancel Request แล้วก็แท็บ Summary, ข้อความแจ้งเตือน/ StatusText ด้านล่างหน้าต่าง |



## 3. ผลการทดสอบ

| กรณี | ผ่าน/ไม่ผ่าน | หมายเหตุ (เฉพาะที่จำเป็น) |
|---|---|---|
| T1 | ผ่าน | สร้างคำขอ M05 ถึง M01 สำเร็จเพราะ M01 ยังไม่มีคำขอ Pending ค้างอยู่ สถานะคำขอใหม่เป็น Pending ตรงตามที่คาดหวัง |
| T2 | ผ่าน | CreateRequest ตรวจพบว่า M01มีคำขอ Pending อยู่แล้ว เลยปฏิเสธแล้วแสดงข้อความ "Already has Pending Request" |
| T3 | ผ่าน | M04 อนุมัติ C01 เป็นเสียงที่ 2 FinalizeIfPossible เปลี่ยนสถานะ C01 เป็น APPROVED และเปลี่ยนบทบาท M02 เป็น EDITOR ทันที |
| T4 | ผ่าน | M05 ไม่อนุมัติให้ C02 เป็นเสียงที่ 2 สถานะของ C02 เลยเปลี่ยนเป็น REJECTED แล้วบทบาท M03 ไม่เปลี่ยน |
| T5 | ผ่าน | M03 ยกเลิก C03 สำเร็จจริง เพราะยังไม่มีตนลงคะแนนโหวต สถานะเลยเปลี่ยนเป็น CANCELLED |
| T6 | ผ่าน | M05 พยายามลงความเห็นต่อ C04 แต่ถูกปฏิเสธ เพราะ M05 คือสมาชิกเป้าหมาย ของคำขอนั้นเอง |

## 4. ความแตกต่างระหว่างแบบที่ออกกับโปรแกรมจริง (ถ้ามี)
ระบุไม่เกิน 3 ข้อ
1. โปรแกรมผ่านการทดสอบแล้วก็ทำตาม T1-T6 ได้ตามที่ออกแบบจริง หลังจากการรันโค้ดใช้งานทดสอบจริงด้วย dotnet restore dotnet run เปิดโปรแกรมและทดสอบ
2. ส่วนที่ผู้ใช้เห็นหรือ View ที่ทำเป็น GUI หน้าต่างเดียวกันมี 5 แท็บ มี Member & Request, Create Request, Submit Decision, Cancel Request, Summary ซึ่งตามการออกแบบก็ตั้งใจไว้ให้มี 1 หน้าต่างและกดแท็บต่าง ๆ เอา
3. ข้อมูลจะรีเซ็ตกลับไปเป็นค่าเริ่มต้นแบบใน seed_data.json ทุกครั้งที่เปิดใหม่ ไม่ได้เก็บข้อมูลไว้

## 5. บันทึกการใช้ Generative AI
หากไม่ได้ใช้ ให้ระบุ **ไม่ได้ใช้ Generative AI**

| เวลาโดยประมาณ | เครื่องมือ | ใช้เพื่ออะไร | นำคำแนะนำไปใช้อย่างไร |
|---|---|---|---|
| 30-45 นาที | Cluade(Antropic) | ใช้เพื่อค้นหาข้อมูล Avalonia UI เพื่อใช้เขียน GUI แต่เตรียมโครงสร้างไว้แล้วเมื่อวันที่ 27/08/69 เพื่อให้สร้าง Desktop GUI ได้ง่ายขึ้น | สร้าง GUI ขึ้นมา |
| 30 นาที | Cluade(Antropic) | ใช้เพื่อค้นหาวิธีใช้ Github ว่าจะ push commit ยังไง | push commit แก้ไข error ของการอัพ github ที่ไม่รู้จัก |
| | | | |
