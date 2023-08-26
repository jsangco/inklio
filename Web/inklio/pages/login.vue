<template>
  <div>
    <p>Login page</p>
    <p>{{ loginResult }}</p>
  </div>
</template>

<script setup>
const { login, login2 } = useIdentity();
const loginResult = await login2(null);
// const loginResult = await login(null);
function listCookies() {
    var theCookies = document.cookie.split(';');
    var aString = '';
    for (var i = 1 ; i <= theCookies.length; i++) {
      aString += i + ' ' + theCookies[i - 1] + "\n";
    }
    return aString;
}
const myCookies = listCookies();
</script>