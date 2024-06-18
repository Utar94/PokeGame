import type { App } from "vue";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

import { library } from "@fortawesome/fontawesome-svg-core";
import {
  faArrowRightFromBracket,
  faArrowRightToBracket,
  faArrowUpRightFromSquare,
  faAt,
  faBan,
  faCheck,
  faChevronLeft,
  faEdit,
  faGlobeAmericas,
  faHome,
  faKey,
  faMagic,
  faPhone,
  faPlus,
  faRotate,
  faSave,
  faTimes,
  faTools,
  faTrashCan,
  faUser,
  faVial,
} from "@fortawesome/free-solid-svg-icons";

library.add(
  faArrowRightFromBracket,
  faArrowRightToBracket,
  faArrowUpRightFromSquare,
  faAt,
  faBan,
  faCheck,
  faChevronLeft,
  faEdit,
  faGlobeAmericas,
  faHome,
  faKey,
  faMagic,
  faPhone,
  faPlus,
  faRotate,
  faSave,
  faTimes,
  faTools,
  faTrashCan,
  faUser,
  faVial,
);

export default function (app: App) {
  app.component("font-awesome-icon", FontAwesomeIcon);
}
