function nobackhfive()
{
    if ($.browser.webkit || $.browser.mozilla)
    {
        if (typeof history.pushState === "function")
        {
            history.pushState("back", null, null);
            window.onpopstate = function window_onPopState() {
                history.pushState('noback', null, null);
            };
        }
    }
}
function noback()
{
    if ($.browser.msie)
    {
        var url = window.location.href;
        history.go(-window.history.length);
        window.location.href = url;
    }

}