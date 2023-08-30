const { login, inklioUser } = useIdentity();

export default function useInklioUser() {
  if (process.server) {
    return inklioUser.value;
  }
  const storedUser = localStorage.getItem("inklioUser");
  if (storedUser !== null) {
    const user = JSON.parse(storedUser);
    inklioUser.value = user;
    return inklioUser;
  }
  return inklioUser
};