import { createRouter, createWebHistory } from "vue-router";

import HomeView from "./views/HomeView.vue";

import { useAccountStore } from "./stores/account";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      name: "Home",
      path: "/",
      component: HomeView,
    },
    // Account
    {
      name: "Profile",
      path: "/profile",
      // route level code-splitting
      // this generates a separate chunk (ProfileView.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import("./views/account/ProfileView.vue"),
    },
    {
      name: "SignIn",
      path: "/sign-in",
      component: () => import("./views/account/SignInView.vue"),
      meta: { isPublic: true },
    },
    {
      name: "SignOut",
      path: "/sign-out",
      component: () => import("./views/account/SignOutView.vue"),
    },
    // Abilities
    {
      name: "AbilityList",
      path: "/abilities",
      component: () => import("./views/abilities/AbilityList.vue"),
    },
    {
      name: "AbilityEdit",
      path: "/abilities/:id",
      component: () => import("./views/abilities/AbilityEdit.vue"),
    },
    // Moves
    {
      name: "MoveList",
      path: "/moves",
      component: () => import("./views/moves/MoveList.vue"),
    },
    {
      name: "MoveEdit",
      path: "/moves/:id",
      component: () => import("./views/moves/MoveEdit.vue"),
    },
    // Regions
    {
      name: "RegionList",
      path: "/regions",
      component: () => import("./views/regions/RegionList.vue"),
    },
    {
      name: "RegionEdit",
      path: "/regions/:id",
      component: () => import("./views/regions/RegionEdit.vue"),
    },
    // NotFound
    {
      name: "NotFound",
      path: "/:pathMatch(.*)*",
      component: () => import("./views/NotFound.vue"),
      meta: { isPublic: true },
    },
  ],
});

router.beforeEach(async (to) => {
  const account = useAccountStore();
  if (!to.meta.isPublic && !account.currentUser) {
    return { name: "SignIn", query: { redirect: to.fullPath } };
  }
});

export default router;
