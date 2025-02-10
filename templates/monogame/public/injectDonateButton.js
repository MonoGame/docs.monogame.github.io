/**
 * Donate button Injector
 * 
 * This script dynamically adds a "Donate" button to the navigation.
 * Since the navbar is dynamically generated **after page load**, it uses a MutationObserver to watch 
 * for the form.icons element to be added to the DOM, injects the donate button as its first child
 * the disconnects the observer.
 */

document.addEventListener('DOMContentLoaded', function() {
    const observer = new MutationObserver(function(mutations) {
        const iconsForm = document.querySelector('form.icons');
        if (iconsForm) {
            observer.disconnect();
            const donateDiv = document.createElement('div');
            donateDiv.innerHTML = '<a class="btn mg-donate-button ms-3" type="button" href="https://monogame.net/donate"><i class="bi bi-heart"></i> Donate</a>';
            iconsForm.insertAdjacentElement('afterbegin', donateDiv);
        }
    });

    observer.observe(document.body, {
        childList: true,
        subtree: true
    });
});