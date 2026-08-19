using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.AiAgentUtilityServices.Prompts
{
    public static class AiAgentPrompts
    {
        public static class GameStateReasoning
        {
            public const string LIST_REWARDS_PROMPT = "分析這張遊戲截圖，目前是否有可以領取的獎勵（紅點）？如果有，請依序列出對應的按鈕描述與座標。";
        }

        public static class SelfHealing
        {
            public const string EXPECT_TO_REDIRECT_TO_WEB_PAGE_PROMPT_WITH_STRING_FORMAT = "腳本原本預期進入『{0}』頁面，但目前畫面顯示異常。{1}: {2}。並給出{3}以恢復流程";
            public static string GetPromptsForExpectedRedirectToWebPage(
                string expectedWebPageName,
                string questionDescription,
                string possibleChoice,
                string expectedResponse
            )
            {
                return string.Format(EXPECT_TO_REDIRECT_TO_WEB_PAGE_PROMPT_WITH_STRING_FORMAT , expectedWebPageName , questionDescription , possibleChoice , expectedResponse);

            }
        }

        public static class ContextualTranslation
        {
            public const string CONTEXTUAL_PROMPT = "提取畫面中所有對話框的文字。如果文字是日文或簡體中文，請翻譯成繁體中文，並判斷這段對話是否包含重要的遊戲任務提示。";
        }
    }
}
