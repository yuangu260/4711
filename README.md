# 4711

## MVP 技术选型（C# + .NET 8 + WPF）

**结论**：MVP 阶段采用 **C# + .NET 8 + WPF**。  

**选型依据**：

1. **与 WinAPI 深度兼容**：全局钩子、热键、剪贴板、窗口置顶、无焦点窗口等能力需要大量 Win32 API 调用，WPF 在 .NET 8 上具备成熟的 P/Invoke 与互操作模型，集成成本最低。
2. **UI 效率与可维护性**：WPF 提供 MVVM 生态、数据绑定、可视化设计器与样式系统，适合快速迭代桌面 UI。
3. **部署与性能**：.NET 8 具备 AOT 选项与原生性能改进，MVP 期可优先使用自包含发布以降低用户环境差异。
4. **生态成熟与团队可复用**：C# 与 Windows 桌面开发人员储备丰富，第三方组件、系统级工具库齐全。
5. **安全与兼容性**：Windows 客户端可直接利用系统安全机制、权限控制与窗口管理能力。

## WinAPI 接口清单（全局钩子/热键/剪贴板/置顶/无焦点窗口）

> 说明：以下为 MVP 所需的 **最小可用** WinAPI 清单，具体封装集中在 `Interop` 层。

### 全局钩子（键盘/鼠标）
- `SetWindowsHookEx`（`WH_KEYBOARD_LL` / `WH_MOUSE_LL`）
- `UnhookWindowsHookEx`
- `CallNextHookEx`
- `GetModuleHandle`
- `GetMessage` / `PeekMessage`（保证钩子消息泵）

### 全局热键
- `RegisterHotKey`
- `UnregisterHotKey`
- `GetMessage` / `PeekMessage`

### 剪贴板
- `OpenClipboard`
- `CloseClipboard`
- `EmptyClipboard`
- `GetClipboardData`
- `SetClipboardData`
- `IsClipboardFormatAvailable`
- `AddClipboardFormatListener`
- `RemoveClipboardFormatListener`

### 窗口置顶/层级管理
- `SetWindowPos`（`HWND_TOPMOST` / `HWND_NOTOPMOST`）
- `GetWindowLong` / `SetWindowLong`
- `GetWindowRect`

### 无焦点窗口（不抢占焦点/任务栏隐藏等）
- `ShowWindow`
- `SetWindowLong` / `SetWindowLongPtr`（`WS_EX_NOACTIVATE`, `WS_EX_TOOLWINDOW`）
- `SetWindowPos`（`SWP_NOACTIVATE`）
- `DwmExtendFrameIntoClientArea`（无边框/透明效果视需要）

## 模块分层图（触发、UI、动作、服务、识别器）

```
┌─────────────────────────────────────────────┐
│                 触发层（Trigger）           │
│ 全局热键 / 选区事件 / 剪贴板变化 / 鼠标钩子 │
└─────────────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────┐
│                 UI 层（UI）                 │
│ WPF Window / Overlay / Panel / Toast       │
└─────────────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────┐
│                动作层（Action）             │
│ 复制 / 改写 / 翻译 / 查询 / 批处理          │
└─────────────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────┐
│                服务层（Service）            │
│ 配置 / 日志 / 热键 / 设备 / 任务管理        │
└─────────────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────┐
│               识别器层（Recognizer）        │
│ OCR / 文本解析 / 语言检测 / 意图识别        │
└─────────────────────────────────────────────┘
```

## MVP 架构文档（含可替换 Provider 设计点）

### 架构目标

- **快速迭代**：最少层级、清晰接口、易替换的 Provider。
- **稳定性**：系统级能力集中在 Interop/Native 层，避免侵入 UI。
- **可扩展性**：动作与识别器可插拔（AI/算法/第三方服务）。

### 分层说明

1. **Trigger 层**  
   接收系统事件（热键/钩子/剪贴板等），只负责转发，不做业务逻辑。
2. **UI 层**  
   仅负责展示与交互，依赖 Actions 的输出。
3. **Action 层**  
   定义用户意图驱动的具体操作，负责流程编排。
4. **Service 层**  
   提供跨模块共享能力（配置、日志、任务调度、权限、缓存）。
5. **Recognizer 层**  
   对输入进行识别（OCR、文本解析、语言检测、意图识别）。

### 可替换 Provider 设计点

| 领域 | Provider 目标 | 替换点示例 |
| --- | --- | --- |
| OCR | `IOcrProvider` | Windows OCR / Tesseract / 云服务 |
| 翻译 | `ITranslateProvider` | Microsoft Translator / OpenAI / 自建 |
| 改写 | `IRewriteProvider` | LLM API / 本地模型 |
| 语言检测 | `ILangDetectProvider` | FastText / CLD3 |
| 热键 | `IHotkeyProvider` | WinAPI / 低权限备用实现 |
| 剪贴板 | `IClipboardProvider` | WinAPI / WPF Clipboard |
| 日志 | `ILoggerProvider` | Serilog / NLog |
| 配置 | `IConfigProvider` | JSON / Registry / SQLite |

### 接口边界建议

- **Interop 层**：仅包含 P/Invoke 与 WinAPI 封装，不允许业务逻辑。
- **Provider 层**：每个能力一个接口，多实现可配置。
- **Action 层**：依赖接口，不依赖具体实现（遵循 DI）。

### MVP 最小实现建议

1. **Trigger**：全局热键 + 剪贴板监听。
2. **UI**：基础窗口 + 结果展示浮窗。
3. **Action**：复制、翻译。
4. **Service**：日志 + 配置。
5. **Recognizer**：OCR（可选，后续扩展）。
