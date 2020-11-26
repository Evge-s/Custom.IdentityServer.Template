var $li = $('.img-list').find('> li'),
    $links = $li.find('> a'),
    $lightbox = $('.lightbox'),
    $overlay = $('.overlay'),
    $prev = $('.prev'),
    $next = $('.next'),
    liIndex,
    targetImg;

// Pre-load for example // PASS
//var imgs = [];
//var imgSources = [
//    '~/img/images/img-1-lg.jpg',
//    '~/img/images/img-2-lg.jpg',
//    '~/img/images/img-3-lg.jpg',
//    '~/img/images/img-4-lg.jpg',
//];

//for (var i = 0; i < imgSource.length; i++) {
//    imgs[i] = new Image();
//    imgs[i] = src.imgSource[i];
//}

function replaceImg(src) {
    $lightbox.find('img').attr('src', src);
}

function getHref(index) {
    return $li.eq(index).find('> a').attr('href');
}

$links.click(function (e) {
    e.preventDefault();
    liIndex = $(this).parent().index();
    targetImg = $(this).attr('href');
    replaceImg(targetImg);
    $lightbox.fadeIn();
});

$overlay.click(function () {
    $lightbox.fadeOut();
});

$next.click(function () {
    if ((liIndex + 1) < $li.length) {
        targetImg = getHref(liIndex + 1);
        liIndex++;
    } else {
        targetImg = getHref(0);
        liIndex = 0;
    }
    replaceImg(targetImg);
});

$prev.click(function () {
    if (liIndex > 0) {
        targetImg = getHref(liIndex - 1);
        liIndex--;
    } else {
        targetImg = getHref($li.length - 1);
        liIndex = $li.length - 1;
    }
    replaceImg(targetImg);
});

