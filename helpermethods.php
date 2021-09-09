 private function _group_by($array, $key)
    {
        $return = array();
        foreach ($array as $val) {
            $return[$val[$key]][] = $val;
        }
        return $return;
    }

    function _group_by_multikey($array, $keys = array())
    {
        $return = array();
        foreach ($array as $val) {
            $final_key = "";
            foreach ($keys as $theKey) {
                $final_key .= $val[$theKey] . "_";
            }
            $return[$final_key][] = $val;
        }
        return $return;
    }
